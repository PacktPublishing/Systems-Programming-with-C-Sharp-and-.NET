using ExtensionLibrary;

using var client = new HttpClient();
try
{
    string url = 
        "https://jsonplaceholder.typicode.com/posts";
    HttpResponseMessage response = 
        await client.GetAsync(url);
    response.EnsureSuccessStatusCode();
    var content = response.Content;
    
    string responseBody = await content.ReadAsStringAsync();
//        await response.Content.ReadAsStringAsync();
    responseBody.Dump(ConsoleColor.Cyan);
}
catch(HttpRequestException ex)
{
    ex.Message.Dump(ConsoleColor.Red);
}