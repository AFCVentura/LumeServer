using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LumeServer.Models.User;

namespace LumeServer.Data.Mappings.User
{
    public class UserDailyProfileMap : IEntityTypeConfiguration<UserDailyProfile>
    {
        public void Configure(EntityTypeBuilder<UserDailyProfile> builder)
        {
            builder.HasKey(udp => udp.Id);

            builder.HasOne(udp => udp.User)
                   .WithMany(u => u.DailyProfiles)
                   .HasForeignKey(udp => udp.UserId);


        }
    }
}
