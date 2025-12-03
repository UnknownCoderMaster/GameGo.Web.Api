using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
	public void Configure(EntityTypeBuilder<Game> builder)
	{
		builder.ToTable("Games");
		builder.HasKey(g => g.Id);

		builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
		builder.Property(g => g.Slug).IsRequired().HasMaxLength(100);
		builder.Property(g => g.Description).HasColumnType("text");
		builder.Property(g => g.CoverImageUrl).HasColumnType("text");
		builder.Property(g => g.IsActive).HasDefaultValue(true);

		builder.HasIndex(g => g.Slug).IsUnique();
		builder.HasIndex(g => g.Name);
	}
}