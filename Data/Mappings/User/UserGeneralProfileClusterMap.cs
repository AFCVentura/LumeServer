using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.User
{
    public class UserGeneralProfileClusterMap : IEntityTypeConfiguration<UserGeneralProfileCluster>
    {
        public void Configure(EntityTypeBuilder<UserGeneralProfileCluster> builder)
        {
            builder.HasKey(ugpc => new { ugpc.ClusterId, ugpc.UserId }); // Chave composta

            builder.HasOne(ugpc => ugpc.Cluster)
                   .WithMany(c => c.UserGeneralProfileClusters)
                   .HasForeignKey(ugpc => ugpc.ClusterId);

            builder.HasOne(ugpc => ugpc.User)
                   .WithMany(u => u.GeneralProfileClusters)
                   .HasForeignKey(ugpc => ugpc.UserId);
        }
    }
}
