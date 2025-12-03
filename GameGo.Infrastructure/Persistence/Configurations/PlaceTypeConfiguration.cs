using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGo.Infrastructure.Persistence.Configurations;

public class PlaceTypeConfiguration : IEntityTypeConfiguration<PlaceType>
{
	public void Configure(EntityTypeBuilder<PlaceType> builder)
	{
		builder.ToTable("PlaceTypes");
		builder.HasKey(pt => pt.Id);

		builder.Property(pt => pt.Name).IsRequired().HasMaxLength(50);
		builder.Property(pt => pt.Slug).IsRequired().HasMaxLength(50);
		builder.Property(pt => pt.Icon).HasMaxLength(100);
		builder.Property(pt => pt.Description).HasMaxLength(300);
		builder.Property(pt => pt.IsActive).HasDefaultValue(true);

		builder.HasIndex(pt => pt.Slug).IsUnique();
	}
}