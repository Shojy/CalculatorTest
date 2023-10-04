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

    [Theory]
    [InlineData(10, 20, 30)]
    [InlineData(45, 10, 55)]
    public async Task Add_GivenSimpleValues_ProducesCorrectResult(int start, int amount, int expected)
    {
        var response = await _client.GetFromJsonAsync<CalculatorResultDto>($"api/Calculator/Add?start={start}&amount={amount}");

        response.Result.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, -20, -10)]
    [InlineData(-45, 10, -35)]
    public async Task Add_GivenNegativeValues_ProducesCorrectResult(int start, int amount, int expected)
    {
        var response = await _client.GetFromJsonAsync<CalculatorResultDto>($"api/Calculator/Add?start={start}&amount={amount}");

        response.Result.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, 20, -10)]
    [InlineData(45, 10, 35)]
    public async Task Subtract_GivenSimpleValues_ProducesCorrectResult(int start, int amount, int expected)
    {
        var response = await _client.GetFromJsonAsync<CalculatorResultDto>($"api/Calculator/Subtract?start={start}&amount={amount}");

        response.Result.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, -20, 30)]
    [InlineData(-45, 10, -55)]
    public async Task Subtract_GivenNegativeValues_ProducesCorrectResult(int start, int amount, int expected)
    {
        var response = await _client.GetFromJsonAsync<CalculatorResultDto>($"api/Calculator/Subtract?start={start}&amount={amount}");

        response.Result.Should().Be(expected);
    }
}