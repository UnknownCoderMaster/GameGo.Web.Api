using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGo.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.HasKey(u => u.Id);

		builder.Property(u => u.Email)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(u => u.PasswordHash)
			.IsRequired()
			.HasMaxLength(255);

		builder.Property(u => u.PhoneNumber)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(u => u.FirstName)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(u => u.LastName)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(u => u.Gender)
			.HasConversion<int>();

		builder.Property(u => u.AvatarUrl)
			.HasColumnType("text");

		builder.Property(u => u.IsActive)
			.IsRequired()
			.HasDefaultValue(true);

		builder.Property(u => u.IsEmailVerified)
			.IsRequired()
			.HasDefaultValue(false);

		builder.Property(u => u.IsPhoneVerified)
			.IsRequired()
			.HasDefaultValue(false);

		builder.Property(u => u.CreatedAt)
			.IsRequired();

		builder.Property(u => u.UpdatedAt)
			.IsRequired();

		// Indexes
		builder.HasIndex(u => u.Email).IsUnique();
		builder.HasIndex(u => u.PhoneNumber).IsUnique();

		// Relationships
		builder.HasMany(u => u.Bookings)
			.WithOne(b => b.User)
			.HasForeignKey(b => b.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(u => u.Ratings)
			.WithOne(r => r.User)
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(u => u.Favourites)
			.WithOne(f => f.User)
			.HasForeignKey(f => f.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(u => u.PlaceOwner)
			.WithOne(po => po.User)
			.HasForeignKey<PlaceOwner>(po => po.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}