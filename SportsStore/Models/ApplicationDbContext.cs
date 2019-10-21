using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SportsStore.Models
{ // each database needs its own context(ApplicationDbContext)
    // this class is a bridging class connecting EFProductRepository and EntityFramework
    public class ApplicationDbContext : DbContext  
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } // calling DbContext's constructor (Parent's constructor)
        public DbSet<Product> Products { get; set; } // the name of table: Products
        public DbSet<Order> Orders { get; set; }
    }
}
