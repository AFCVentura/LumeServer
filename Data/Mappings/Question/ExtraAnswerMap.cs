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

            builder.Property(ea => ea.MinVoteAverage)
                   .HasDefaultValue(0.0f);
            builder.Property(ea => ea.MaxVoteAverage)
                   .HasDefaultValue(10.1f);
            builder.Property(ea => ea.MinVoteCount)
                   .HasDefaultValue(0);
            builder.Property(ea => ea.MaxVoteCount)
                   .HasDefaultValue(1000000);
            builder.Property(ea => ea.MinYear)
                   .HasDefaultValue(0);
            builder.Property(ea => ea.MaxYear)
                   .HasDefaultValue(5000);
            builder.Property(u => u.MinDuration)
                   .HasDefaultValue(0);
            builder.Property(u => u.MaxDuration)
                   .HasDefaultValue(1000);
        }
    }
}
