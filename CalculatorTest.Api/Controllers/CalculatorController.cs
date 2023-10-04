using CalculatorTest.Api.Models;
using CalculatorTest.Lib;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorTest.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{

    private readonly ILogger<CalculatorController> _logger;
    private readonly ISimpleCalculator _calculator;

    public CalculatorController(ILogger<CalculatorController> logger, ISimpleCalculator calculator)
    {
        _logger = logger;
        _calculator = calculator;
    }

    [HttpGet("[action]")]
    public CalculatorResultDto Add(int start, int amount)
    {
        _logger.LogDebug("Entering {class}.{method}", nameof(CalculatorController), nameof(Add));
        var result = _calculator.Add(start, amount);

        return new CalculatorResultDto(result);
    }

    [HttpGet("[action]")]
    public CalculatorResultDto Subtract(int start, int amount)
    {
        _logger.LogDebug("Entering {class}.{method}", nameof(CalculatorController), nameof(Subtract));
        var result = _calculator.Subtract(start, amount);

        return new CalculatorResultDto(result);
    }
}

