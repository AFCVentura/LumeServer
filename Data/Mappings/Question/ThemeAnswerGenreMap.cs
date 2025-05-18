using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.Question
{
    public class ThemeAnswerGenreMap : IEntityTypeConfiguration<ThemeAnswerGenre>
    {
        public void Configure(EntityTypeBuilder<ThemeAnswerGenre> builder)
        {
            builder.HasKey(tag => new { tag.GenreId, tag.ThemeAnswerId });

            builder.HasOne(tag => tag.ThemeAnswer)
                   .WithMany(ta => ta.ThemeAnswerGenres)
                   .HasForeignKey(tag => tag.ThemeAnswerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tag => tag.Genre)
                    .WithMany(k => k.ThemeAnswerGenres)
                    .HasForeignKey(tag => tag.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
