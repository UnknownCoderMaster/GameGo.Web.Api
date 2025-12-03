using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGo.Infrastructure.Persistence.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
	public void Configure(EntityTypeBuilder<Place> builder)
	{
		builder.ToTable("Places");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(150);

		builder.Property(p => p.Slug)
			.IsRequired()
			.HasMaxLength(150);

		builder.Property(p => p.Description)
			.HasColumnType("text");

		builder.Property(p => p.Address)
			.IsRequired()
			.HasMaxLength(400);

		builder.Property(p => p.Latitude)
			.IsRequired()
			.HasPrecision(10, 8);

		builder.Property(p => p.Longitude)
			.IsRequired()
			.HasPrecision(11, 8);

		builder.Property(p => p.PhoneNumber)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(p => p.Email)
			.HasMaxLength(100);

		builder.Property(p => p.Website)
			.HasMaxLength(200);

		builder.Property(p => p.InstagramUsername)
			.HasMaxLength(50);

		builder.Property(p => p.TelegramUsername)
			.HasMaxLength(50);

		builder.Property(p => p.AverageRating)
			.HasPrecision(3, 2)
			.HasDefaultValue(0);

		builder.Property(p => p.TotalRatings)
			.HasDefaultValue(0);

		builder.Property(p => p.TotalBookings)
			.HasDefaultValue(0);

		builder.Property(p => p.IsActive)
			.IsRequired()
			.HasDefaultValue(true);

		builder.Property(p => p.IsVerified)
			.IsRequired()
			.HasDefaultValue(false);

		// Indexes
		builder.HasIndex(p => p.Slug).IsUnique();
		builder.HasIndex(p => p.PlaceTypeId);
		builder.HasIndex(p => p.OwnerId);
		builder.HasIndex(p => new { p.Latitude, p.Longitude });
		builder.HasIndex(p => p.AverageRating);

		// Relationships
		builder.HasOne(p => p.PlaceType)
			.WithMany(pt => pt.Places)
			.HasForeignKey(p => p.PlaceTypeId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(p => p.Owner)
			.WithMany()
			.HasForeignKey(p => p.OwnerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(p => p.PlaceImages)
			.WithOne(pi => pi.Place)
			.HasForeignKey(pi => pi.PlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(p => p.PlaceFeatures)
			.WithOne(pf => pf.Place)
			.HasForeignKey(pf => pf.PlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(p => p.WorkingHours)
			.WithOne(wh => wh.Place)
			.HasForeignKey(wh => wh.PlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(p => p.Services)
			.WithOne(s => s.Place)
			.HasForeignKey(s => s.PlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(p => p.PlaceGames)
			.WithOne(pg => pg.Place)
			.HasForeignKey(pg => pg.PlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(p => p.Bookings)
			.WithOne(b => b.Place)
			.HasForeignKey(b => b.PlaceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(p => p.Ratings)
			.WithOne(r => r.Place)
			.HasForeignKey(r => r.PlaceId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}