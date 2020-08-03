using Microsoft.EntityFrameworkCore;
using Neutron.Core;

namespace Neutron.Infrastructure
{
    public class NeutronDbContext : DbContext
    {
        public NeutronDbContext(DbContextOptions<NeutronDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Event>()
                .HasKey(it => it.Id);

            modelBuilder
                .Entity<Event>()
                .Property(it => it.Name)
                .IsRequired();

            modelBuilder
                .Entity<Event>()
                .Property(it => it.Target)
                .IsRequired();

            modelBuilder
                .Entity<Event>()
                .Property<string>("UserId")
                .HasColumnType("TEXT");

            modelBuilder
                .Entity<Event>()
                .Ignore(it => it.TimeLeft);

            modelBuilder
                .Entity<Event>()
                .Ignore(it => it.HasArrived);

            modelBuilder
                .Entity<Event>()
                .Ignore(it => it.HasNotArrived);
        }
    }
}
