using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameDeviceConfiguration : IEntityTypeConfiguration<GameDevice>
{
	public void Configure(EntityTypeBuilder<GameDevice> builder)
	{
		builder.ToTable("GameDevices");
		builder.HasKey(gd => gd.Id);

		builder.HasIndex(gd => new { gd.GameId, gd.DeviceId }).IsUnique();

		builder.HasOne(gd => gd.Game)
			.WithMany(g => g.GameDevices)
			.HasForeignKey(gd => gd.GameId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(gd => gd.Device)
			.WithMany(d => d.GameDevices)
			.HasForeignKey(gd => gd.DeviceId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}