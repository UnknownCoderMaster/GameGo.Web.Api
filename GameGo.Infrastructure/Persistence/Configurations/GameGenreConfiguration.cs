using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
{
	public void Configure(EntityTypeBuilder<GameGenre> builder)
	{
		builder.ToTable("GameGenres");
		builder.HasKey(gg => gg.Id);

		builder.HasIndex(gg => new { gg.GameId, gg.GenreId }).IsUnique();

		builder.HasOne(gg => gg.Game)
			.WithMany(g => g.GameGenres)
			.HasForeignKey(gg => gg.GameId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(gg => gg.Genre)
			.WithMany(g => g.GameGenres)
			.HasForeignKey(gg => gg.GenreId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}