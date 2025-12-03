using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGo.Infrastructure.Persistence.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
	public void Configure(EntityTypeBuilder<Rating> builder)
	{
		builder.ToTable("Ratings");

		builder.HasKey(r => r.Id);

		builder.Property(r => r.Score)
			.IsRequired();

		builder.Property(r => r.Review)
			.HasColumnType("text");

		builder.Property(r => r.Pros)
			.HasMaxLength(500);

		builder.Property(r => r.Cons)
			.HasMaxLength(500);

		builder.Property(r => r.IsAnonymous)
			.HasDefaultValue(false);

		builder.Property(r => r.IsVerified)
			.HasDefaultValue(false);

		builder.Property(r => r.HelpfulCount)
			.HasDefaultValue(0);

		// Indexes
		builder.HasIndex(r => new { r.PlaceId, r.Score });
		builder.HasIndex(r => new { r.UserId, r.PlaceId }).IsUnique();
		builder.HasIndex(r => r.BookingId);

		// Relationships
		builder.HasOne(r => r.Booking)
			.WithMany()
			.HasForeignKey(r => r.BookingId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(r => r.RatingHelpfuls)
			.WithOne(rh => rh.Rating)
			.HasForeignKey(rh => rh.RatingId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}