using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Covaciu_Carla_Proiect.Models;

namespace Covaciu_Carla_Proiect.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) :base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Distributor> Distributors{ get; set; }
        public DbSet<DistributedPhone> DistributedPhones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Phone>().ToTable("Phone");
            modelBuilder.Entity<Distributor>().ToTable("Distributor");
            modelBuilder.Entity<DistributedPhone>().ToTable("DistributedPhone");
            modelBuilder.Entity<DistributedPhone>().HasKey(c => new { c.PhoneID, c.DistributorID });//configureaza cheia primara compusa
        }
    }
}
