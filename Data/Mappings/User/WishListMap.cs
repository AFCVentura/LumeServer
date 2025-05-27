using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LumeServer.Data.Mappings.User
{
    public class WishListMap : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder.HasKey(wil => new { wil.MovieId, wil.UserId }); // Chave composta

            builder.HasOne(wil => wil.Movie)
                   .WithMany(m => m.WishList)
                   .HasForeignKey(wil => wil.MovieId);

            builder.HasOne(wil => wil.User)
                   .WithMany(u => u.WishList)
                   .HasForeignKey(wil => wil.UserId);
        }
    }
}
