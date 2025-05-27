using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class GenreMap : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(k => k.MovieGenres)
                   .WithOne(mk => mk.Genre)
                   .HasForeignKey(mk => mk.GenreId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
