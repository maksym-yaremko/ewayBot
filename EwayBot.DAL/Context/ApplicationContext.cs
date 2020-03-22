using EwayBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EwayBot.DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Stop> Stops { get; set; }
        public DbSet<LastUserMessage> LastUserMessages { get; set; }

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
            modelBuilder.Entity<LastUserMessage>().HasIndex(u => u.ChatId).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Eway;Trusted_Connection=True");
            }
        }
    }
}
