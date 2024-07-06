using System.Net;
using System.Net.Http.Headers;
using ExtensionLibrary;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace _08_SmartHttpsClient
{
    internal static class HttpClientFactory
    {
        private static HttpClient? _instance;
        private static AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private static AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public static HttpClient? Instance
        {
            get
            {
                if (_instance == null) CreateInstance();
                return _instance;
            }
        }

        private static void CreateInstance()
        {
            var handler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                UseProxy = false
            };

            _instance = new HttpClient(handler);
            _instance.DefaultRequestHeaders.Clear();
            _instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _instance.DefaultRequestHeaders.Add("User-Agent", "SystemProgrammersApp");
            
            _instance.Timeout = TimeSpan.FromSeconds(5);
            
            // Set up Retry Policy
            SetupRetryPolicy();
            SetupCircuitBreakerPolicy();
        }

        private static void SetupRetryPolicy()
        {
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    3, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (outcome, timeSpan, retryCount, context) =>
                    {
                        $"Request failed with {outcome.Result.StatusCode}.".Dump(ConsoleColor.Red);
                        $"Waiting {timeSpan} before next retry.".Dump(ConsoleColor.Red);
                        $"Retry attempt {retryCount}.".Dump(ConsoleColor.Red);
                    });
        }

        private static void SetupCircuitBreakerPolicy()
        {
            _circuitBreakerPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(2),
                    (result, timespan) =>
                    {
                            $"Opening circuit for {timespan}. ExecutionResult: {result.Exception?.Message ?? result.Result.StatusCode.ToString()}".Dump(ConsoleColor.DarkYellow);
                    },
                    () => { "Circuit closed. Reset.".Dump(ConsoleColor.DarkYellow); },
                    () => { "Circuit half-open. Next call is a trial.".Dump(ConsoleColor.DarkYellow); });
        }
        
        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _retryPolicy.ExecuteAsync(
                () => _circuitBreakerPolicy.ExecuteAsync(
                () => _instance.GetAsync(url)));
        }
    }
}
