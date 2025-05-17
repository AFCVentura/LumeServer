using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ExtraAnswerSpokenLanguageMap : IEntityTypeConfiguration<ExtraAnswerSpokenLanguage>
    {
        public void Configure(EntityTypeBuilder<ExtraAnswerSpokenLanguage> builder)
        {
            builder.HasKey(k => new { k.ExtraAnswerId, k.SpokenLanguageId });

            builder.HasOne(k => k.ExtraAnswer)
                   .WithMany(m => m.ExtraAnswerSpokenLanguages)
                   .HasForeignKey(k => k.ExtraAnswerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(k => k.SpokenLanguage)
                    .WithMany(m => m.ExtraAnswerSpokenLanguages)
                    .HasForeignKey(k => k.SpokenLanguageId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
