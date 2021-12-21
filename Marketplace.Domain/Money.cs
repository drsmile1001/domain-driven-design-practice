namespace Marketplace.Domain;

public record Money(decimal Amount)
{
    public Money Add(Money summand) => new(Amount + summand.Amount);
    public Money Subtract(Money subtrahend) => new(Amount - subtrahend.Amount);

    public static Money operator +(Money a, Money b) => a.Add(b);
    public static Money operator -(Money a, Money b) => a.Subtract(b);
}
