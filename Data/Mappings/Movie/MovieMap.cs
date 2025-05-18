using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class MovieMap : IEntityTypeConfiguration<LumeServer.Models.Movie.Movie>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LumeServer.Models.Movie.Movie> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title).HasMaxLength(255);

            builder.HasOne(m => m.Cluster)
                   .WithMany(c => c.Movies)
                   .HasForeignKey(m => m.ClusterId);

            builder.HasMany(m => m.MovieGenres)
                   .WithOne(mg => mg.Movie)
                   .HasForeignKey(mg => mg.MovieId);

            builder.HasMany(m => m.MovieKeywords)
                   .WithOne(mk => mk.Movie)
                   .HasForeignKey(mk => mk.MovieId);

            builder.HasMany(m => m.MovieProductionCompanies)
                   .WithOne(mk => mk.Movie)
                   .HasForeignKey(mk => mk.MovieId);

            builder.HasMany(m => m.MovieProductionCountries)
                   .WithOne(mk => mk.Movie)
                   .HasForeignKey(mk => mk.MovieId);

            builder.HasMany(m => m.MovieSpokenLanguages)
                   .WithOne(mk => mk.Movie)
                   .HasForeignKey(mk => mk.MovieId);

            // WishList
            builder.HasMany(m => m.WishList)
                .WithOne(wil => wil.Movie)
                .HasForeignKey(wil => wil.MovieId);

            // WatchedList
            builder.HasMany(m => m.WatchedList)
                .WithOne(wal => wal.Movie)
                .HasForeignKey(wal => wal.MovieId);


        }
    }
}
