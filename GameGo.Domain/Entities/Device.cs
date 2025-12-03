using GameGo.Domain.Common;
using System.Collections.Generic;

public class Device : AuditableEntity
{
	public string Name { get; set; } = null!;
	public string Slug { get; set; } = null!;

	public virtual ICollection<GameDevice> GameDevices { get; set; } = new List<GameDevice>();

	//private Device() { }
}