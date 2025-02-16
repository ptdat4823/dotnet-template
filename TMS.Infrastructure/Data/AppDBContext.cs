using Microsoft.EntityFrameworkCore;
using TMS.Domain.Entities;
namespace TMS.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
    }
}