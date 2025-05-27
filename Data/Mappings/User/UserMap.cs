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

            // GeneralProfileProductionCountries
            builder.HasMany(u => u.GeneralProfileProductionCountries)
                .WithOne(ugpc => ugpc.User)
                .HasForeignKey(ugpc => ugpc.UserId);

            // GeneralProfileSpokenLanguages
            builder.HasMany(u => u.GeneralProfileSpokenLanguages)
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

            builder.Property(u => u.MinVoteAverage)
                   .HasDefaultValue(0.0f);
            builder.Property(u => u.MaxVoteAverage)
                   .HasDefaultValue(10.1f);
            builder.Property(u => u.MinVoteCount)
                   .HasDefaultValue(0);
            builder.Property(u => u.MaxVoteCount)
                   .HasDefaultValue(1000000);
            builder.Property(u => u.MinYear)
                   .HasDefaultValue(0);
            builder.Property(u => u.MaxYear)
                   .HasDefaultValue(5000);
            builder.Property(u => u.MinDuration)
                   .HasDefaultValue(0);
            builder.Property(u => u.MaxDuration)
                   .HasDefaultValue(1000);
        }
    }
}
