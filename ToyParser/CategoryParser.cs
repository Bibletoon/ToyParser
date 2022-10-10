using AngleSharp;
using AngleSharp.Dom;

namespace ToyParser;

public class CategoryParser
{
    private readonly PageLoader _loader;

    public CategoryParser(PageLoader loader)
    {
        _loader = loader;
    }

    public List<Product> Parse(string categoryUrl)
    {
        var document = _loader.LoadPage($"{categoryUrl}?count=45").Result;

        var pagesCount = int.Parse(document.QuerySelector(".pagination > .page-item:nth-last-child(2) > a").TextContent.Trim());
        var productsLinks = new List<string>();

        for (int i = 1; i < pagesCount; i++)
        {
            document = _loader.LoadPage($"{categoryUrl}?count=45&PAGEN_5={i}").Result;
            productsLinks.AddRange(ParseProductLinks(document));
        }

        var parser = new ProductParser(_loader);
        var product = productsLinks.Select(x => parser.Parse($"https://toy.ru{x}")).ToList();

        return product;
    }

    private List<string> ParseProductLinks(IDocument document)
    {
        return document.QuerySelectorAll(".product-card > .row > div:first-child > a").Select(x => x.GetAttribute("href")).ToList();
    }
}