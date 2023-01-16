using System;
using Balogh_David_Project.Models;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Balogh_David_Project.Data
{
    public class LaptopContext : DbContext
    {
        public LaptopContext(DbContextOptions<LaptopContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributedLaptop> DistributedLaptops { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Laptop>().ToTable("Laptop");
            modelBuilder.Entity<Manufacturer>().ToTable("Manufacturer");
            modelBuilder.Entity<Distributor>().ToTable("Distributor");
            modelBuilder.Entity<DistributedLaptop>().ToTable("DistributedLaptop");
            modelBuilder.Entity<DistributedLaptop>()
            .HasKey(c => new { c.LaptopID, c.DistributorID });
        }
    }
}

