using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class MovieGenreMap : IEntityTypeConfiguration<MovieGenre>
    {
        public void Configure(EntityTypeBuilder<MovieGenre> builder)
        {
            builder.HasKey(mk => new { mk.MovieId, mk.GenreId }); // Chave composta

            builder.HasOne(mk => mk.Movie)
                   .WithMany(m => m.MovieGenres)
                   .HasForeignKey(mk => mk.MovieId);

            builder.HasOne(mk => mk.Genre)
                   .WithMany(k => k.MovieGenres)
                   .HasForeignKey(mk => mk.GenreId);
        }
    }
}
