using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ThemeAnswerMap : IEntityTypeConfiguration<ThemeAnswer>
    {
        public void Configure(EntityTypeBuilder<ThemeAnswer> builder)
        {
            builder.HasKey(ta => ta.Id);

            builder.HasOne(ta => ta.ThemeQuestion)
                   .WithMany(tq => tq.ThemeAnswers)
                   .HasForeignKey(ta => ta.ThemeQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ta => ta.ThemeAnswerGenres)
                     .WithOne(tag => tag.ThemeAnswer)
                     .HasForeignKey(tag => tag.ThemeAnswerId)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ta => ta.ThemeAnswerKeywords)
                        .WithOne(tak => tak.ThemeAnswer)
                        .HasForeignKey(tak => tak.ThemeAnswerId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }    
}
