using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext {
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<ProductRating>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.ProductRatings)
            .HasForeignKey(pr => pr.ProductId);

        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProductId);

        modelBuilder.Entity<PriceHistory>()
            .HasOne(ph => ph.Product)
            .WithMany(p => p.PriceHistories)
            .HasForeignKey(ph => ph.ProductId);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId);


        modelBuilder.Entity<Product>().OwnsOne(p => p.Price, p =>
        {
            p.Property(m => m.Amount).HasColumnName("PriceAmount").HasPrecision(10, 2);
            p.Property(m => m.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
        });
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }
}