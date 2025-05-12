using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.User
{
    public class WatchedListMap : IEntityTypeConfiguration<WatchedList>
    {
        public void Configure(EntityTypeBuilder<WatchedList> builder)
        {
            builder.HasKey(wal => new { wal.MovieId, wal.UserId }); // Chave composta

            builder.HasOne(wal => wal.Movie)
                   .WithMany(m => m.WatchedList)
                   .HasForeignKey(wal => wal.MovieId);

            builder.HasOne(wal => wal.User)
                   .WithMany(u => u.WatchedList)
                   .HasForeignKey(wal => wal.UserId);
        }
    }
}
