using GameGo.Domain.Common;
using GameGo.Domain.Entities;

public class PlaceImage : AuditableEntity
{
	public long PlaceId { get; set; }
	public string ImageUrl { get; set; } = null!;
	public bool IsPrimary { get; set; }
	public int DisplayOrder { get; set; }

	public virtual Place Place { get; set; } = null!;

	//private PlaceImage() { }
}