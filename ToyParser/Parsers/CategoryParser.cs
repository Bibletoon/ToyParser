using System.Collections.Concurrent;
using AngleSharp.Dom;
using ToyParser.Models;
using ToyParser.Tools;

namespace ToyParser.Parsers;

public class CategoryParser
{
    private readonly PageLoader _loader;

    public CategoryParser(PageLoader loader)
    {
        _loader = loader;
    }

    public async Task<List<Product>> Parse(string categoryUrl, Region? region = null)
    {
        var document = await _loader.LoadPage($"{categoryUrl}?count=45", region?.Cookie);

        var pagesCount = ParsePagesCount(document);
        var productsLinks = await GetAllProductsLinks(categoryUrl, region, pagesCount);

        var products = new ConcurrentBag<Product>();
        await Parallel.ForEachAsync(productsLinks, async (productUrl, token) =>
        {
            var parser = new ProductParser(_loader);
            var product = await parser.ParseAsync($"https://toy.ru{productUrl}", region);
            products.Add(product);
        });

        return products.ToList();
    }

    private async Task<List<string>> GetAllProductsLinks(string categoryUrl, Region? region, int pagesCount)
    {
        var productsLinks = new List<string>();

        for (var i = 1; i <= pagesCount; i++)
        {
            var document = await _loader.LoadPage($"{categoryUrl}?count=45&PAGEN_5={i}", region?.Cookie);
            productsLinks.AddRange(ParseProductLinks(document));
        }

        return productsLinks;
    }

    private List<string> ParseProductLinks(IDocument document)
    {
        return document.QuerySelectorAll(".product-card > .row > div:first-child > a")
            .Select(x => x.GetAttribute("href"))
            .ToList();
    }

    private int ParsePagesCount(IDocument document)
    {
        var pagesCountString =
            document.QuerySelector(".pagination > .page-item:nth-last-child(2) > a")?
                .TextContent
                .Trim() ?? "1";
        var pagesCount = int.Parse(pagesCountString);
        return pagesCount;
    }
}