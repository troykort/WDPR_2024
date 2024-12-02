using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CustomerBase> Customers { get; set; }
        public DbSet<BusinessCustomer> BusinessCustomers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relaties tussen BusinessCustomer en Employees
            modelBuilder.Entity<BusinessCustomer>()
                .HasMany(b => b.Employees)
                .WithOne(e => e.BusinessCustomer)
                .HasForeignKey(e => e.BusinessCustomerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

