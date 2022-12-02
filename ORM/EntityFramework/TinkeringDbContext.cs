using Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class TinkeringDbContext : DbContext, IUnitOfWork
    {
        private readonly string connectionString;

        public TinkeringDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Child> Children { get; set; }

        public async Task Commit(Aggregate aggregate)
        {
            foreach (var evnt in aggregate.Events())
            {
                switch (evnt)
                {
                    // Need this to tell Entity Framework to remove Person from the database
                    case PersonDeletedEvent pde:
                        Remove(pde.PersonDeleteData);
                        break;
                }
            }
            await SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable("Person");
            modelBuilder.Entity<Person>()
                .Property(p => p.PersonId)
                .HasColumnName("Id");
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

            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Address>()
                .HasKey(a => a.PersonId);

            modelBuilder.Entity<Child>().ToTable("Child");
            modelBuilder.Entity<Child>()
                .Property(p => p.ChildId)
                .HasColumnName("Id");
            modelBuilder.Entity<Child>()
                .HasKey(c => c.ChildId);
        }
    }
}
