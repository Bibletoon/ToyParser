using System.Text.RegularExpressions;
using AngleSharp.Dom;

namespace ToyParser;

public class ProductParser
{
    private static readonly Regex PriceRegex = new(@"[\d, \,]+", RegexOptions.Compiled);
    private readonly PageLoader _loader;

    public ProductParser(PageLoader loader)
    {
        _loader = loader;
    }

    public async Task<Product> ParseAsync(string pageUrl)
    {
        var document = await _loader.LoadPage(pageUrl);
        var product = new Product();

        product.Region = document.QuerySelector(".select-city-link > a").TextContent.Trim();
        product.Name = document.QuerySelector("h1").TextContent.Trim();
        product.Breadcrumbs = document.QuerySelectorAll(".breadcrumb-item:not(:last-child)")
            .Select(e => e.TextContent.Trim())
            .ToArray();

        if (IsAvailable(document))
        {
            product.IsAvailable = true;
            product.Price = ParsePrice(document.QuerySelector(".price").TextContent);
            var oldPrice = document.QuerySelector(".old-price")?.TextContent;
            product.OldPrice = oldPrice is null ? null : ParsePrice(oldPrice);
        }

        product.Images = document.QuerySelectorAll(".card-slider-for > div > a > img")
            .Select(e => e.GetAttribute("src").Trim())
            .ToArray();

        return product;
    }

    private bool IsAvailable(IDocument document)
    {
        return document.QuerySelector(".net-v-nalichii") is null;
    }

    private double ParsePrice(string price)
    {
        return double.Parse(PriceRegex.Match(price).Value);
    }
}