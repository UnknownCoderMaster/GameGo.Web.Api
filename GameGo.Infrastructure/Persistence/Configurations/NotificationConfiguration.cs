using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)
	{
		builder.ToTable("Notifications");
		builder.HasKey(n => n.Id);

		builder.Property(n => n.Title).IsRequired().HasMaxLength(150);
		builder.Property(n => n.Message).IsRequired().HasColumnType("text");
		builder.Property(n => n.Type).IsRequired().HasConversion<int>();
		builder.Property(n => n.RelatedEntityType).HasMaxLength(50);
		builder.Property(n => n.IsRead).HasDefaultValue(false);

		builder.HasIndex(n => new { n.UserId, n.IsRead });
		builder.HasIndex(n => new { n.UserId, n.CreatedAt });

		builder.HasOne(n => n.User)
			.WithMany()
			.HasForeignKey(n => n.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}