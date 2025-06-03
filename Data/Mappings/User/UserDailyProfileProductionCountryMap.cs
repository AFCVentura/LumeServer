using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.User
{
    public class UserDailyProfileProductionCountryMap : IEntityTypeConfiguration<UserDailyProfileProductionCountry>
    {
        public void Configure(EntityTypeBuilder<UserDailyProfileProductionCountry> builder)
        {
            builder.HasKey(udppc => new { udppc.ProductionCountryId, udppc.UserDailyProfileId }); // Chave composta

            builder.HasOne(udppc => udppc.ProductionCountry)
                   .WithMany(sl => sl.UserDailyProfileProductionCountries)
                   .HasForeignKey(udppc => udppc.ProductionCountryId);

            builder.HasOne(udppc => udppc.UserDailyProfile)
                   .WithMany(udp => udp.UserDailyProfileProductionCountries)
                   .HasForeignKey(udppc => udppc.UserDailyProfileId);
        }
    }
}
