using GameGo.Domain.Common;
using GameGo.Domain.Entities;
using System.Collections.Generic;

public class PlaceOwner : AuditableEntity
{
	public long UserId { get;  set; }
	public string CompanyName { get;  set; }
	public string TaxId { get;  set; }
	public string BusinessLicense { get;  set; }
	public bool IsVerified { get; set; }

	public virtual User User { get; set; } = null!;
	public virtual ICollection<Place> Places { get;  set; } = new List<Place>();

	public PlaceOwner() { }
}