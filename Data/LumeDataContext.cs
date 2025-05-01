using LumeServer.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LumeServer.Data
{
    public class LumeDataContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        private readonly string CONNECTION_STRING;
        public LumeDataContext(DbContextOptions<LumeDataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.LogTo(Console.WriteLine);
        }

    }
}
