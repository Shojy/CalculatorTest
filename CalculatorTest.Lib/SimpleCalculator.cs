using JetBrains.Annotations;

namespace CalculatorTest.Lib;

[UsedImplicitly]
public interface ISimpleCalculator
{
    /// <summary>
    /// Add 2 numbers together and return the result
    /// </summary>
    /// <param name="start"></param>
    /// <param name="amount"></param>
    /// <returns>The sum of start and amount</returns>
    /// <exception cref="OverflowException">Thrown if the result is higher than int.MaxValue or lower than int.MinValue</exception>
    [Pure]
    [UsedImplicitly]
    int Add(int start, int amount);

    /// <summary>
    /// Subtracts an amount from the starting value
    /// </summary>
    /// <param name="start">The starting value</param>
    /// <param name="amount">The amount to subtract</param>
    /// <returns></returns>
    /// <exception cref="OverflowException">Thrown if the result is higher than int.MaxValue or lower than int.MinValue</exception>
    [Pure]
    [UsedImplicitly]
    int Subtract(int start, int amount);
}

[UsedImplicitly]
public class SimpleCalculator : ISimpleCalculator
{
    [Pure]
    public int Add(int start, int amount)
    {
        return start + amount;
    }

    [Pure]
    public int Subtract(int start, int amount)
    {
        // In some scenarios, subtracting a negative number may wish to result in an error. In this
        // case, it is assumed that negative values are acceptable and thus treated normally without
        // a filter or error.
        return start - amount;
    }
}