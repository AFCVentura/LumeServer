using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.User
{
    public class UserGeneralProfileProductionCountryMap : IEntityTypeConfiguration<UserGeneralProfileProductionCountry>
    {
        public void Configure(EntityTypeBuilder<UserGeneralProfileProductionCountry> builder)
        {
            builder.HasKey(udppc => new { udppc.ProductionCountryId, udppc.UserId }); // Chave composta

            builder.HasOne(udppc => udppc.ProductionCountry)
                   .WithMany(sl => sl.UserGeneralProfileProductionCountries)
                   .HasForeignKey(udppc => udppc.ProductionCountryId);

            builder.HasOne(udppc => udppc.User)
                   .WithMany(udp => udp.GeneralProfileProductionCountries)
                   .HasForeignKey(udppc => udppc.UserId);
        }
    }
}
