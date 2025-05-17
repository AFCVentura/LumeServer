using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ExtraAnswerMap : IEntityTypeConfiguration<ExtraAnswer>
    {
        public void Configure(EntityTypeBuilder<ExtraAnswer> builder)
        {
            builder.HasKey(ea => ea.Id);

            builder.HasOne(ea => ea.Question)
                   .WithMany(eq => eq.Answers)
                   .HasForeignKey(ea => ea.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ea => ea.ExtraAnswerProductionCountries)
                     .WithOne(eapc => eapc.ExtraAnswer)
                     .HasForeignKey(eapc => eapc.ExtraAnswerId)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ea => ea.ExtraAnswerSpokenLanguages)
                        .WithOne(easl => easl.ExtraAnswer)
                        .HasForeignKey(easl => easl.ExtraAnswerId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
