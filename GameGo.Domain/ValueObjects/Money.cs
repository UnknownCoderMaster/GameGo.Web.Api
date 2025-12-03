using System;

namespace GameGo.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
	public decimal Amount { get; private set; }
	public string Currency { get; private set; }

	private Money() { Currency = "UZS"; }

	public Money(decimal amount, string currency = "UZS")
	{
		if (amount < 0)
			throw new ArgumentException("Amount cannot be negative", nameof(amount));

		if (string.IsNullOrWhiteSpace(currency))
			throw new ArgumentException("Currency cannot be empty", nameof(currency));

		Amount = amount;
		Currency = currency.ToUpper();
	}

	public Money Add(Money other)
	{
		if (Currency != other.Currency)
			throw new InvalidOperationException("Cannot add money with different currencies");

		return new Money(Amount + other.Amount, Currency);
	}

	public Money Subtract(Money other)
	{
		if (Currency != other.Currency)
			throw new InvalidOperationException("Cannot subtract money with different currencies");

		return new Money(Amount - other.Amount, Currency);
	}

	public bool Equals(Money other)
	{
		if (other is null) return false;
		return Amount == other.Amount && Currency == other.Currency;
	}

	public override bool Equals(object obj) => Equals(obj as Money);

	public override int GetHashCode() => HashCode.Combine(Amount, Currency);

	public static Money operator +(Money left, Money right) => left.Add(right);
	public static Money operator -(Money left, Money right) => left.Subtract(right);
}