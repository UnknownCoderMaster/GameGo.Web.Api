using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGo.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
	public void Configure(EntityTypeBuilder<Booking> builder)
	{
		builder.ToTable("Bookings");

		builder.HasKey(b => b.Id);

		builder.Property(b => b.BookingDate)
			.IsRequired();

		builder.Property(b => b.StartTime)
			.IsRequired();

		builder.Property(b => b.EndTime)
			.IsRequired();

		builder.Property(b => b.NumberOfPeople)
			.IsRequired();

		builder.Property(b => b.Status)
			.IsRequired()
			.HasConversion<int>();

		builder.Property(b => b.TotalPrice)
			.HasPrecision(10, 2);

		builder.Property(b => b.SpecialRequests)
			.HasColumnType("text");

		builder.Property(b => b.CancellationReason)
			.HasMaxLength(500);

		// Indexes
		builder.HasIndex(b => new { b.UserId, b.Status });
		builder.HasIndex(b => new { b.PlaceId, b.BookingDate, b.Status });
		builder.HasIndex(b => new { b.PlaceId, b.Status, b.BookingDate, b.StartTime });

		// Relationships
		builder.HasOne(b => b.Service)
			.WithMany()
			.HasForeignKey(b => b.ServiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(b => b.History)
			.WithOne(bh => bh.Booking)
			.HasForeignKey(bh => bh.BookingId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}