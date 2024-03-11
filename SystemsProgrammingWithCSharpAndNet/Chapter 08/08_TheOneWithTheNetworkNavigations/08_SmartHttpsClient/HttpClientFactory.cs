using System.Net;
using System.Net.Http.Headers;

namespace _08_SmartHttpsClient
{
    internal static class HttpClientFactory
    {
        public static HttpClient? Instance
        {
            get
            {
                if (_instance == null) CreateInstance();
                return _instance;
            }
        }
        private static HttpClient? _instance;

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
        }
    }
}
