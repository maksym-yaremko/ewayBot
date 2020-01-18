using EwayBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EwayBot.DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Stop> Stops { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
