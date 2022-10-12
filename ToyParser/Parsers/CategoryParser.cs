using System.Collections.Concurrent;
using AngleSharp.Dom;

namespace ToyParser;

public class CategoryParser
{
    private readonly PageLoader _loader;

    public CategoryParser(PageLoader loader)
    {
        _loader = loader;
    }

    public async Task<List<Product>> Parse(string categoryUrl)
    {
        var document = await _loader.LoadPage($"{categoryUrl}?count=45");

        var pagesCount = int.Parse(
            document.QuerySelector(".pagination > .page-item:nth-last-child(2) > a")
                .TextContent
                .Trim());
        var productsLinks = new List<string>();

        for (var i = 1; i <= pagesCount; i++)
        {
            document = await _loader.LoadPage($"{categoryUrl}?count=45&PAGEN_5={i}");
            productsLinks.AddRange(ParseProductLinks(document));
        }

        var products = new ConcurrentBag<Product>();
        await Parallel.ForEachAsync(productsLinks, async (productUrl, token) =>
        {
            var parser = new ProductParser(_loader);
            var product = await parser.ParseAsync($"https://toy.ru{productUrl}");
            products.Add(product);
        });

        return products.ToList();
    }

    private List<string> ParseProductLinks(IDocument document)
    {
        return document.QuerySelectorAll(".product-card > .row > div:first-child > a")
            .Select(x => x.GetAttribute("href"))
            .ToList();
    }
}