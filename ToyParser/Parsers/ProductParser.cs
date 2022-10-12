using System.Text.RegularExpressions;
using AngleSharp.Dom;
using ToyParser.Models;
using ToyParser.Tools;

namespace ToyParser.Parsers;

public class ProductParser
{
    private static readonly Regex PriceRegex = new(@"[\d, \,]+", RegexOptions.Compiled);
    private readonly PageLoader _loader;

    public ProductParser(PageLoader loader)
    {
        _loader = loader;
    }

    public async Task<Product> ParseAsync(string pageUrl, Region? region)
    {
        var document = await _loader.LoadPage(pageUrl, region?.Cookie);
        var product = new Product();

        product.Region = document.QuerySelector(".select-city-link > a").TextContent.Trim();
        product.Name = document.QuerySelector("h1").TextContent.Trim();
        product.Breadcrumbs = document.QuerySelectorAll(".breadcrumb-item:not(:last-child)")
            .Select(e => e.TextContent.Trim())
            .ToArray();
        product.Url = pageUrl;

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