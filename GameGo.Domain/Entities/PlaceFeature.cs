using GameGo.Domain.Common;
using GameGo.Domain.Entities;

public class PlaceFeature : AuditableEntity
{
	public long PlaceId { get; private set; }
	public string FeatureName { get; private set; } = null!;
	public bool IsAvailable { get; private set; }

	public virtual Place Place { get; private set; } = null!;

	private PlaceFeature() { }
}