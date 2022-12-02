using Microsoft.EntityFrameworkCore;
using ORM.Controllers;

namespace EntityFramework
{
    public class TinkeringDbContext : DbContext
    {
        private readonly string connectionString;

        public TinkeringDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Child> Children { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasKey(p => p.PersonId);
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Address)
                .WithOne(a => a.Person)
                .HasForeignKey<Address>(a => a.PersonId);
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Children)
                .WithOne(c => c.Person)
                .HasForeignKey(c => c.PersonId);
        }
    }
}
