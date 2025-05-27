using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class ProductionCountryMap : IEntityTypeConfiguration<ProductionCountry>
    {
        public void Configure(EntityTypeBuilder<ProductionCountry> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // MovieProductionCountries
            builder.HasMany(k => k.MovieProductionCountries)
                   .WithOne(mk => mk.ProductionCountry)
                   .HasForeignKey(mk => mk.ProductionCountryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ExtraAnswerProductionCountries
            builder.HasMany(k => k.ExtraAnswerProductionCountries)
                .WithOne(mk => mk.ProductionCountry)
                .HasForeignKey(mk => mk.ProductionCountryId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserGeneralProfileProductionCountries
            builder.HasMany(k => k.UserGeneralProfileProductionCountries)
                .WithOne(mk => mk.ProductionCountry)
                .HasForeignKey(mk => mk.ProductionCountryId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserDailyProfileProductionCountries
            builder.HasMany(k => k.UserDailyProfileProductionCountries)
                .WithOne(mk => mk.ProductionCountry)
                .HasForeignKey(mk => mk.ProductionCountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
