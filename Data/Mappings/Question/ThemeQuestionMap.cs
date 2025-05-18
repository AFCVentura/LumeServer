using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ThemeQuestionMap : IEntityTypeConfiguration<ThemeQuestion>
    {
        public void Configure(EntityTypeBuilder<ThemeQuestion> builder)
        {
            builder.HasKey(tq => tq.Id);

            builder.HasMany(tq => tq.ThemeAnswers)
                .WithOne(ta => ta.ThemeQuestion)
                .HasForeignKey(ta => ta.ThemeQuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
