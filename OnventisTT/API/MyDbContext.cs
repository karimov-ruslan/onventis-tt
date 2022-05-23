using Microsoft.EntityFrameworkCore;
using OnventisTT.Infrastructure.Entities;

namespace OnventisTT
{
    public class MyDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MyDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}
