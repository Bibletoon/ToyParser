using System.Collections.Concurrent;

namespace ToyParser.Tools;

public static class HttpClientPool
{
    private static readonly ConcurrentBag<HttpClient> Clients = new ConcurrentBag<HttpClient>();

    public static HttpClient Get() => Clients.TryTake(out var client) ? client : new HttpClient();

    public static void Return(HttpClient client) => Clients.Add(client);
}