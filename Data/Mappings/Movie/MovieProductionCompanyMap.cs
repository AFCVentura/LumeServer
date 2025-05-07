using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class MovieProductionCompanyMap : IEntityTypeConfiguration<MovieProductionCompany>
    {
        public void Configure(EntityTypeBuilder<MovieProductionCompany> builder)
        {
            builder.HasKey(mk => new { mk.MovieId, mk.ProductionCompanyId }); // Chave composta

            builder.HasOne(mk => mk.Movie)
                   .WithMany(m => m.MovieProductionCompanies)
                   .HasForeignKey(mk => mk.MovieId);

            builder.HasOne(mk => mk.ProductionCompany)
                   .WithMany(k => k.MovieProductionCompanies)
                   .HasForeignKey(mk => mk.KeywordId);
        }
    }
}
