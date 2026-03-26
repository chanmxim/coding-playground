using assignment01.Areas.OrderManagement.Models;
using assignment01.Areas.ProductManagement.Models;
using assignment01.Areas.Identity.Models;
using assignment01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<CartItem> ShoppingCartItems { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // One-to-many between Category-Product entities
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);
        
        // Many-to-many with linking table between Product-Order entities
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.OrderItemId);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(oi => oi.OrdersItems)
            .HasForeignKey(oi => oi.ProductId);

        // One-to-many between Product-CartItem
        modelBuilder.Entity<Product>()
            .HasMany(ci => ci.CartItems)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CartItems)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);
        
        // One-to-many between User-Order entities
        modelBuilder.Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);
        
        
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(name: "User");
        });
        
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Role");
        });
        
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable(name: "UserRole");
        });
        
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable(name: "UserClaim");
        });
        
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable(name: "UserLogin");
        });
        
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable(name: "UserToken");
        });
        
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable(name: "RoleClaim");
        });


        
    }
}