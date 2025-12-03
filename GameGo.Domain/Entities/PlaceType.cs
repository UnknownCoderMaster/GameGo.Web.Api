using GameGo.Domain.Common;
using System.Collections.Generic;

namespace GameGo.Domain.Entities;

public class PlaceType : AuditableEntity
{
	public string Name { get; private set; } = null!;
	public string Slug { get; private set; } = null!;
	public string Icon { get; private set; }
	public string Description { get; private set; }
	public bool IsActive { get; private set; }

	// Navigation Properties
	public virtual ICollection<Place> Places { get; private set; } = new List<Place>();

	private PlaceType() { }

	public static PlaceType Create(string name, string slug, string icon = null, string description = null)
	{
		return new PlaceType
		{
			Name = name,
			Slug = slug.ToLower(),
			Icon = icon,
			Description = description,
			IsActive = true
		};
	}
}