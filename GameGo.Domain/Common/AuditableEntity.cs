using System;

namespace GameGo.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	protected AuditableEntity() : base() { }
}