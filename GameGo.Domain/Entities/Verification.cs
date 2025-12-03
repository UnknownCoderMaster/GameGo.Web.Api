using GameGo.Domain.Common;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;
using System;

public class Verification : AuditableEntity
{
	public long UserId { get; set; }
	public VerificationType VerificationType { get; set; }
	public string Code { get; set; } = null!;
	public DateTime ExpiresAt { get; set; }
	public bool IsUsed { get; set; }

	public virtual User User { get; set; } = null!;

	public Verification() { }
}