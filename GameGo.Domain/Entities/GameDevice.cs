using GameGo.Domain.Common;

public class GameDevice : AuditableEntity
{
	public long GameId { get; private set; }
	public long DeviceId { get; private set; }

	public virtual Game Game { get; private set; } = null!;
	public virtual Device Device { get; private set; } = null!;

	private GameDevice() { }
}