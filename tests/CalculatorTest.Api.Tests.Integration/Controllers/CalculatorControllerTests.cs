using FluentAssertions;

namespace CalculatorTest.Api.Tests.Integration.Controllers;


// Using a copy of the expected model ensures the contract is not silently violated by refactoring in the api project
public readonly record struct CalculatorResultDto(int Result);

[Collection("Shared collection")]
public class CalculatorControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;

    public CalculatorControllerTests(ApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
    }
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Theory]
    [InlineData("api/Calculator/Add")]
    [InlineData("api/Calculator/Subtract")]
    public async Task PublicEndpoints_DoExist(string endpoint)
    {
        var response = await _client.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
    }
}