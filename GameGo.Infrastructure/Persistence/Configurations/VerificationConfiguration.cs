using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
	public void Configure(EntityTypeBuilder<Verification> builder)
	{
		builder.ToTable("Verifications");
		builder.HasKey(v => v.Id);

		builder.Property(v => v.VerificationType).IsRequired().HasConversion<int>();
		builder.Property(v => v.Code).IsRequired().HasMaxLength(10);
		builder.Property(v => v.ExpiresAt).IsRequired();
		builder.Property(v => v.IsUsed).HasDefaultValue(false);

		builder.HasIndex(v => new { v.UserId, v.VerificationType });
		builder.HasIndex(v => new { v.Code, v.ExpiresAt });

		builder.HasOne(v => v.User)
			.WithMany()
			.HasForeignKey(v => v.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}