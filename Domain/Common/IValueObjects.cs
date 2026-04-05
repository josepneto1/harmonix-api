namespace Harmonix.Domain.Common;

public interface IValueObject<T, TValue> where T : IValueObject<T, TValue>
{
    TValue Value { get; }
    static abstract T FromDbConfig(TValue value);
}
