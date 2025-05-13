using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.User
{
    public class UserMap : IEntityTypeConfiguration<LumeServer.Models.User.User>
    {
        public void Configure(EntityTypeBuilder<LumeServer.Models.User.User> builder)
        {
            // DailyProfiles
            builder.HasMany(u => u.DailyProfiles)
                .WithOne(udp => udp.User)
                .HasForeignKey(udp => udp.UserId);

            // GeneralProfileClusters
            builder.HasMany(u => u.GeneralProfileClusters)
                .WithOne(ugpc => ugpc.User)
                .HasForeignKey(ugpc => ugpc.UserId);

            // WishList
            builder.HasMany(u => u.WishList)
                .WithOne(wil => wil.User)
                .HasForeignKey(wil => wil.UserId);

            // WatchedList
            builder.HasMany(u => u.WatchedList)
                .WithOne(wal => wal.User)
                .HasForeignKey(wal => wal.UserId);
        }
    }
}
