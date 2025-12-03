using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PlaceGameConfiguration : IEntityTypeConfiguration<PlaceGame>
{
	public void Configure(EntityTypeBuilder<PlaceGame> builder)
	{
		builder.ToTable("PlaceGames");
		builder.HasKey(pg => pg.Id);

		builder.Property(pg => pg.IsAvailable).HasDefaultValue(true);

		builder.HasIndex(pg => new { pg.PlaceId, pg.GameId }).IsUnique();
		builder.HasIndex(pg => new { pg.PlaceId, pg.IsAvailable });

		builder.HasOne(pg => pg.Game)
			.WithMany(g => g.PlaceGames)
			.HasForeignKey(pg => pg.GameId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}