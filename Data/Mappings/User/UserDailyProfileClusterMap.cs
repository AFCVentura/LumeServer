using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.User
{
    public class UserDailyProfileClusterMap : IEntityTypeConfiguration<UserDailyProfileCluster>
    {
        public void Configure(EntityTypeBuilder<UserDailyProfileCluster> builder)
        {
            builder.HasKey(udpc => new { udpc.UserDailyProfileId, udpc.ClusterId }); // Chave composta

            builder.HasOne(udpc => udpc.UserDailyProfile)
                   .WithMany(udp => udp.UserDailyProfileClusters)
                   .HasForeignKey(udpc => udpc.UserDailyProfileId);

            builder.HasOne(udpc => udpc.Cluster)
                   .WithMany(c => c.UserDailyProfileClusters)
                   .HasForeignKey(udpc => udpc.ClusterId);
        }
    }
}
