using FastEndpoints;

namespace CachingCore.Modules;

public class CacheResponse
{
    public string Message { get; set; }
}

public class CacheTestEndpoint : Endpoint<EndpointWithoutRequest<CacheResponse>>
{
    public override void Configure()
    {
        Get("/CacheTest");
        Summary(s =>
        {
            s.Summary = "Cache test";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(EndpointWithoutRequest<CacheResponse> req, CancellationToken ct)
    {
        await Send.OkAsync("Cache test");
    }
}