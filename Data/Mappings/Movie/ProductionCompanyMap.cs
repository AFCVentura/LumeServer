using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class ProductionCompanyMap : IEntityTypeConfiguration<ProductionCompany>
    {
        public void Configure(EntityTypeBuilder<ProductionCompany> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(k => k.MovieProductionCompanies)
                   .WithOne(mk => mk.ProductionCompany)
                   .HasForeignKey(mk => mk.ProductionCompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
