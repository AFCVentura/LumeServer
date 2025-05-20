using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LumeServer.Models.User;

namespace LumeServer.Data.Mappings.User
{
    public class UserDailyProfileSpokenLanguageMap : IEntityTypeConfiguration<UserDailyProfileSpokenLanguage>
    {
        public void Configure(EntityTypeBuilder<UserDailyProfileSpokenLanguage> builder)
        {
            builder.HasKey(udpsl => new { udpsl.SpokenLanguageId, udpsl.UserDailyProfileId }); // Chave composta

            builder.HasOne(udpsl => udpsl.SpokenLanguage)
                   .WithMany(sl => sl.UserDailyProfileSpokenLanguages)
                   .HasForeignKey(udpsl => udpsl.SpokenLanguageId);

            builder.HasOne(udpsl => udpsl.UserDailyProfile)
                   .WithMany(udp => udp.UserDailyProfileSpokenLanguages)
                   .HasForeignKey(udpsl => udpsl.UserDailyProfileId);
        }
    }
}
