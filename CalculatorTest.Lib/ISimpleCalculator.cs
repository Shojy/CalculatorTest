using JetBrains.Annotations;

namespace CalculatorTest.Lib;

[UsedImplicitly]
public interface ISimpleCalculator
{
    int Add(int start, int amount);
    int Subtract(int start, int amount);
}

[UsedImplicitly]
public class SimpleCalculator : ISimpleCalculator
{
    public int Add(int start, int amount)
    {
        throw new NotImplementedException();
    }

    public int Subtract(int start, int amount)
    {
        throw new NotImplementedException();
    }
}