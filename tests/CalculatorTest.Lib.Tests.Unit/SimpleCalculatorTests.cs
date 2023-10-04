using FluentAssertions;

namespace CalculatorTest.Lib.Tests.Unit;

public class SimpleCalculatorTests
{
    private readonly SimpleCalculator _sut;

    public SimpleCalculatorTests()
    {
        _sut = new SimpleCalculator();
    }

    [Fact]
    public void Instance_DoesConstruct()
    {
        _sut.Should()
            .BeAssignableTo(typeof(ISimpleCalculator))
            .And.NotBeNull();
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(1, 2, 3)]
    [InlineData(2, 2, 4)]
    public void Add_GivenSimpleNumbers_ProducesCorrectResult(int start, int value, int actual)
    {
        var result = _sut.Add(start, value);

        result.Should().Be(actual);
    }

    [Theory]
    [InlineData(1, -1, 0)]
    [InlineData(1, -2, -1)]
    [InlineData(-2, 2, 0)]
    [InlineData(-2, -2, -4)]
    public void Add_GivenNegativeNumbers_ProducesCorrectResult(int start, int value, int actual)
    {
        var result = _sut.Add(start, value);

        result.Should().Be(actual);
    }


    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(2, 1, 1)]
    [InlineData(2, 2, 0)]
    public void Subtract_GivenSimpleNumbers_ProducesCorrectResult(int start, int value, int actual)
    {
        var result = _sut.Subtract(start, value);

        result.Should().Be(actual);
    }

    [Theory]
    [InlineData(1, -1, 2)]
    [InlineData(1, -2, 3)]
    [InlineData(-2, 2, -4)]
    [InlineData(-2, -2, -0)]
    public void Subtract_GivenNegativeNumbers_ProducesCorrectResult(int start, int value, int actual)
    {
        var result = _sut.Subtract(start, value);

        result.Should().Be(actual);
    }

    [Theory]
    [InlineData(2147483647, 1)]
    [InlineData(1, 2147483647)]
    [InlineData(-2147483648, -1)]
    [InlineData(-1, -2147483648)]
    public void Add_GivenExtremeNumbers_ThrowsException(int start, int value)
    {
        _sut.Invoking(x => x.Add(start, value)).Should().Throw<OverflowException>();
    }
}
