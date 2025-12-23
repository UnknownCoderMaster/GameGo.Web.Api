using GameGo.Domain.Common;

namespace GameGo.Domain.Entities;

public class Service : AuditableEntity
{
	public long PlaceId { get; private set; }
	public string Name { get; private set; } = null!;
	public string Description { get; private set; }
	public decimal Price { get; private set; }
	public string Currency { get; private set; } = "UZS";
	public int? DurationMinutes { get; private set; }
	public int Capacity { get; private set; }
	public bool IsActive { get; private set; }

	public virtual Place Place { get; private set; } = null!;

	private Service() { }

	public static Service Create(
		long placeId,
		string name,
		decimal price,
		string description = null,
		string currency = "UZS",
		int? durationMinutes = null,
		int capacity = 1)
	{
		return new Service
		{
			PlaceId = placeId,
			Name = name,
			Description = description,
			Price = price,
			Currency = currency,
			DurationMinutes = durationMinutes,
			Capacity = capacity,
			IsActive = true
		};
	}

	public void Update(
		string name,
		decimal price,
		string description = null,
		int? durationMinutes = null,
		int capacity = 1)
	{
		Name = name;
		Description = description;
		Price = price;
		DurationMinutes = durationMinutes;
		Capacity = capacity;
	}

	public void Activate() => IsActive = true;

	public void Deactivate() => IsActive = false;
}