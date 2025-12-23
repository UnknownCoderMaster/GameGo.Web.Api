using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameGo.Infrastructure.Persistence.Configurations;

public class WorkingHoursConfiguration : IEntityTypeConfiguration<WorkingHours>
{
	public void Configure(EntityTypeBuilder<WorkingHours> builder)
	{
		builder.ToTable("WorkingHours");
		builder.HasKey(wh => wh.Id);

		builder.Property(wh => wh.DayOfWeek).IsRequired().HasConversion<int>();
		builder.Property(wh => wh.OpenTime).IsRequired();
		builder.Property(wh => wh.CloseTime).IsRequired();
		builder.Property(wh => wh.IsClosed).HasDefaultValue(false);

		builder.HasIndex(wh => new { wh.PlaceId, wh.DayOfWeek });
	}
}