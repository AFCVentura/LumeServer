using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ExtraQuestionMap : IEntityTypeConfiguration<ExtraQuestion>
    {
        public void Configure(EntityTypeBuilder<ExtraQuestion> builder)
        {
            builder.HasKey(eq => eq.Id);

            builder.HasMany(eq => eq.Answers)
                .WithOne(ea => ea.Question)
                .HasForeignKey(ea => ea.QuestionId);
        }
    }
}
