using Microsoft.EntityFrameworkCore;
using ABC_Retail_v3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ABC_Retail_v3.Data
{
    public class ApplicationDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CraftUsers> Craft_Users { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Products>()
                .Property(u => u.availability)
                .HasComputedColumnSql("case when Stock_count = 0 then 'Out Of Stock' else 'Available' end");

            modelBuilder
                .Entity<Transactions>()
                .HasOne(a => a.Product) // Assuming Transactions has a Cart navigation property
                .WithMany() // Assuming Cart has multiple Transactions
                .HasForeignKey(a => a.Product_id) // Assuming Transactions has a CartId foreign key
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Transactions>()
                .HasOne(t => t.Cart) // Assuming Transactions has a Cart navigation property
                .WithMany() // Assuming Cart has multiple Transactions
                .HasForeignKey(t => t.CartId) // Assuming Transactions has a CartId foreign key
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Transactions>()
                .HasOne(s => s.Customers)  // Assuming Transactions has a navigation property called Customer
                .WithMany()               // Assuming a Customer can have many Transactions
                .HasForeignKey(s => s.Customer_id) // Assuming Transactions has a Customer_id foreign key
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
