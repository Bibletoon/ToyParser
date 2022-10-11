using System.Net;
using AngleSharp;
using AngleSharp.Dom;

namespace ToyParser;

public class PageLoader
{
    private readonly IConfiguration _config;

    public PageLoader(HttpClient client, IConfiguration configuration)
    {
        _config = configuration;
    }

    public async Task<IDocument> LoadPage(string url)
    {
        var client = new HttpClient();
        var context = BrowsingContext.New(_config);
        while (true)
        {
            var response = await client.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                await Task.Delay(100);
                continue;
            }
            var content = await response.Content.ReadAsStringAsync();
            return await context.OpenAsync(res => res.Content(content));
        }
    }
}