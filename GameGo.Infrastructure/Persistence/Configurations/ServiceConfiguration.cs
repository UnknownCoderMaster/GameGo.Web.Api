using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
	public void Configure(EntityTypeBuilder<Service> builder)
	{
		builder.ToTable("Services");
		builder.HasKey(s => s.Id);

		builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
		builder.Property(s => s.Description).HasMaxLength(500);
		builder.Property(s => s.Price).HasPrecision(10, 2);
		builder.Property(s => s.Currency).HasMaxLength(3).HasDefaultValue("UZS");
		builder.Property(s => s.Capacity).HasDefaultValue(1);
		builder.Property(s => s.IsActive).HasDefaultValue(true);

		builder.HasIndex(s => new { s.PlaceId, s.IsActive });
	}
}