using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.User
{
    public class UserGeneralProfileSpokenLanguageMap : IEntityTypeConfiguration<UserGeneralProfileSpokenLanguage>
    {
        public void Configure(EntityTypeBuilder<UserGeneralProfileSpokenLanguage> builder)
        {
            builder.HasKey(udppc => new { udppc.SpokenLanguageId, udppc.UserId }); // Chave composta

            builder.HasOne(udppc => udppc.SpokenLanguage)
                   .WithMany(sl => sl.UserGeneralProfileSpokenLanguages)
                   .HasForeignKey(udppc => udppc.SpokenLanguageId);

            builder.HasOne(udppc => udppc.User)
                   .WithMany(udp => udp.GeneralProfileSpokenLanguages)
                   .HasForeignKey(udppc => udppc.UserId);
        }
    }
}
