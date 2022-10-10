using AngleSharp;
using AngleSharp.Dom;

namespace ToyParser;

public class PageLoader
{
    private readonly HttpClient _client;
    private readonly IBrowsingContext _context;

    public PageLoader(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _context = BrowsingContext.New(configuration);
    }

    public async Task<IDocument> LoadPage(string url)
    {
        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return await _context.OpenAsync(req => req.Content(content));
    }
}