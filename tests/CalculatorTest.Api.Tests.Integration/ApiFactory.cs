using Microsoft.AspNetCore.Mvc.Testing;

namespace CalculatorTest.Api.Tests.Integration;

public class ApiFactory : WebApplicationFactory<ICalculatorTestApiAssemblyMarker>, IAsyncLifetime
{

    public HttpClient HttpClient { get; private set; } = default!;

    public Task InitializeAsync()
    {
        HttpClient = GetHttpClient();

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected HttpClient GetHttpClient()
    {
        var client = WithWebHostBuilder(builder => { })
            .CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

        return client;
    }
}