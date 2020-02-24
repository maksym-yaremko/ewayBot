using EwayBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EwayBot.DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Stop> Stops { get; set; }

        public ApplicationContext()
        {
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stop>().HasIndex(x => x.Title);
        }
    }
}
