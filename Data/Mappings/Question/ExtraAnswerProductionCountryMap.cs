using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Question
{
    public class ExtraAnswerProductionCountryMap : IEntityTypeConfiguration<ExtraAnswerProductionCountry>
    {
        public void Configure(EntityTypeBuilder<ExtraAnswerProductionCountry> builder)
        {
            builder.HasKey(k => new { k.ExtraAnswerId, k.ProductionCountryId });

            builder.HasOne(k => k.ExtraAnswer)
                   .WithMany(m => m.ExtraAnswerProductionCountries)
                   .HasForeignKey(k => k.ExtraAnswerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(k => k.ProductionCountry)
                    .WithMany(m => m.ExtraAnswerProductionCountries)
                    .HasForeignKey(k => k.ProductionCountryId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
