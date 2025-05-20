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

            builder.HasMany(udp => udp.UserDailyProfileSpokenLanguages)
                   .WithOne(udpsl => udpsl.UserDailyProfile)
                   .HasForeignKey(udpsl => udpsl.UserDailyProfileId);

            builder.HasMany(udp => udp.UserDailyProfileProductionCountries)
                   .WithOne(udppc => udppc.UserDailyProfile)
                   .HasForeignKey(udppc => udppc.UserDailyProfileId);

            builder.Property(udp => udp.MinVoteAverage)
                   .HasDefaultValue(0.0f);
            builder.Property(udp => udp.MaxVoteAverage)
                   .HasDefaultValue(10.1f);
            builder.Property(udp => udp.MinVoteCount)
                   .HasDefaultValue(0);
            builder.Property(udp => udp.MaxVoteCount)
                   .HasDefaultValue(1000000);
            builder.Property(udp => udp.MinYear)
                   .HasDefaultValue(0);
            builder.Property(udp => udp.MaxYear)
                   .HasDefaultValue(5000);
            builder.Property(u => u.MinDuration)
                   .HasDefaultValue(0);
            builder.Property(u => u.MaxDuration)
                   .HasDefaultValue(1000);
        }
    }
}
