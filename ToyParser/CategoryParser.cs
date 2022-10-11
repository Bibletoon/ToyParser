using System.Collections.Concurrent;
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

    public async Task<List<Product>> Parse(string categoryUrl)
    {
        var document = await _loader.LoadPage($"{categoryUrl}?count=45");

        var pagesCount = int.Parse(document.QuerySelector(".pagination > .page-item:nth-last-child(2) > a").TextContent.Trim());
        var productsLinks = new List<string>();

        for (int i = 1; i < pagesCount; i++)
        {
            document = await _loader.LoadPage($"{categoryUrl}?count=45&PAGEN_5={i}");
            productsLinks.AddRange(ParseProductLinks(document));
        }

        var products = new ConcurrentBag<Product>();
        await Parallel.ForEachAsync(productsLinks, async (s, token) =>
        {
            var parser = new ProductParser(_loader);
            await Retry(x => parser.ParseAsync($"https://toy.ru{x}"), s, 5, products);
        });

        return products.ToList();
    }

    private List<string> ParseProductLinks(IDocument document)
    {
        return document.QuerySelectorAll(".product-card > .row > div:first-child > a").Select(x => x.GetAttribute("href")).ToList();
    }

    private static async Task Retry<T, TResult>(Func<T, Task<TResult>> func, T arg, int retryCount, ConcurrentBag<TResult> resultBag)
    {
        while (true)
        {
            try
            {
                var result = await func.Invoke(arg);
                resultBag.Add(result);
                break;
            }
            catch
            {
                Thread.Sleep(100);
                if (retryCount-- > 0) continue;

                Console.WriteLine("Unable to load page.");
            }
        }
    }
}