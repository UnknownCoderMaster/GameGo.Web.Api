using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
	public void Configure(EntityTypeBuilder<Favourite> builder)
	{
		builder.ToTable("Favourites");
		builder.HasKey(f => f.Id);

		builder.HasIndex(f => new { f.UserId, f.PlaceId }).IsUnique();
		builder.HasIndex(f => f.UserId);

		builder.HasOne(f => f.Place)
			.WithMany()
			.HasForeignKey(f => f.PlaceId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}