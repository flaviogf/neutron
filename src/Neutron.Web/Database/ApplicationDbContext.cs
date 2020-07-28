using Microsoft.EntityFrameworkCore;
using Neutron.Web.Models;

namespace Neutron.Web.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasKey(it => it.Id);

            modelBuilder.Entity<Event>()
                .Property(it => it.Name)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(it => it.Target)
                .IsRequired();
        }
    }
}
