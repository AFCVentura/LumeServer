using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class SpokenLanguageMap : IEntityTypeConfiguration<SpokenLanguage>
    {
        public void Configure(EntityTypeBuilder<SpokenLanguage> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // MovieSpokenLanguages
            builder.HasMany(k => k.MovieSpokenLanguages)
                   .WithOne(mk => mk.SpokenLanguage)
                   .HasForeignKey(mk => mk.SpokenLanguageId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ExtraAnswerSpokenLanguages
            builder.HasMany(k => k.ExtraAnswerSpokenLanguages)
                   .WithOne(mk => mk.SpokenLanguage)
                   .HasForeignKey(mk => mk.SpokenLanguageId)
                   .OnDelete(DeleteBehavior.Cascade);

            // UserGeneralProfileSpokenLanguages
            builder.HasMany(k => k.UserGeneralProfileSpokenLanguages)
                   .WithOne(mk => mk.SpokenLanguage)
                   .HasForeignKey(mk => mk.SpokenLanguageId)
                   .OnDelete(DeleteBehavior.Cascade);

            // UserDailyProfileSpokenLanguages
            builder.HasMany(k => k.UserDailyProfileSpokenLanguages)
                   .WithOne(mk => mk.SpokenLanguage)
                   .HasForeignKey(mk => mk.SpokenLanguageId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
