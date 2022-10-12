﻿using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using ToyParser.Tools;

namespace ToyParser;

public class PageLoader
{
    private readonly IConfiguration _configuration;

    public PageLoader(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IDocument> LoadPage(string url)
    {
        var client = HttpClientPool.Get();
        var context = BrowsingContext.New(_configuration);
        while (true)
        {
            var response = await client.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                await Task.Delay(100);
                continue;
            }

            var content = await response.Content.ReadAsStringAsync();
            HttpClientPool.Return(client);
            return await context.OpenAsync(res => res.Header("Content-Type", "text/html; charset=utf-8").Content(content));
        }
    }
}