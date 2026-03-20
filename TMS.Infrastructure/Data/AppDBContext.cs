using Microsoft.EntityFrameworkCore;
using TMS.Domain.Entities;
namespace TMS.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        public DbSet<User1> User1Db { get; set; }

        public DbSet<Order> OrderDb { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
    }
}