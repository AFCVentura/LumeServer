using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ThemeAnswerKeywordMap : IEntityTypeConfiguration<ThemeAnswerKeyword>
    {
        public void Configure(EntityTypeBuilder<ThemeAnswerKeyword> builder)
        {
            builder.HasKey(tak => new { tak.ThemeAnswerId, tak.KeywordId });

            builder.HasOne(tak => tak.ThemeAnswer)
                   .WithMany(ta => ta.ThemeAnswerKeywords)
                   .HasForeignKey(tak => tak.ThemeAnswerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tak => tak.Keyword)
                    .WithMany(k => k.ThemeAnswerKeywords)
                    .HasForeignKey(tak => tak.KeywordId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
    {
    }
}
