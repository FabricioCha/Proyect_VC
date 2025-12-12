using Microsoft.EntityFrameworkCore;
using UtilesArequipa.Application.Interfaces.Persistence;
using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
    public virtual DbSet<Kit> Kits { get; set; } = null!;
    public virtual DbSet<KitItem> KitItems { get; set; } = null!;
    public virtual DbSet<Cart> Carts { get; set; } = null!;
    public virtual DbSet<CartItem> CartItems { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
    public virtual DbSet<DeliveryRoute> DeliveryRoutes { get; set; } = null!;
    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
    public virtual DbSet<InventoryLog> InventoryLogs { get; set; } = null!;
    public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
    public virtual DbSet<SupplierOrder> SupplierOrders { get; set; } = null!;
    public virtual DbSet<SupplierOrderItem> SupplierOrderItems { get; set; } = null!;
    public virtual DbSet<SchoolList> SchoolLists { get; set; } = null!;
    public virtual DbSet<SchoolListItem> SchoolListItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(80);
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(80);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(120);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
            entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(20).HasDefaultValue("customer");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("addresses");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Street).HasColumnName("street").HasMaxLength(180);
            entity.Property(e => e.District).HasColumnName("district").HasMaxLength(80);
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(80).HasDefaultValue("Arequipa");
            entity.Property(e => e.Reference).HasColumnName("reference").HasMaxLength(200);
            entity.Property(e => e.IsDefault).HasColumnName("is_default").HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
            entity.Property(e => e.Slug).HasColumnName("slug").HasMaxLength(120);
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Sku).HasColumnName("sku").HasMaxLength(100);
            entity.HasIndex(e => e.Sku).IsUnique();
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Brand).HasColumnName("brand").HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnName("price").HasPrecision(10, 2);
            entity.Property(e => e.CostPrice).HasColumnName("cost_price").HasPrecision(10, 2).HasDefaultValue(0);
            entity.Property(e => e.Stock).HasColumnName("stock").HasDefaultValue(0);
            entity.Property(e => e.StockMin).HasColumnName("stock_min").HasDefaultValue(0);
            entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.RowVersion).HasColumnName("xmin").HasColumnType("xid").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("product_images");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Url).HasColumnName("url").HasMaxLength(255);
            entity.Property(e => e.IsMain).HasColumnName("is_main").HasDefaultValue(false);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Kit>(entity =>
        {
            entity.ToTable("kits");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price").HasPrecision(10, 2);
            entity.Property(e => e.Grade).HasColumnName("grade").HasMaxLength(50);
            entity.Property(e => e.SchoolName).HasColumnName("school_name").HasMaxLength(150);
            entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<KitItem>(entity =>
        {
            entity.ToTable("kit_items");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KitId).HasColumnName("kit_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity").HasDefaultValue(1);

            entity.HasIndex(e => new { e.KitId, e.ProductId }).IsUnique();

            entity.HasOne(d => d.Kit)
                .WithMany(p => p.KitItems)
                .HasForeignKey(d => d.KitId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.KitItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("cart");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("cart_items");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.KitId).HasColumnName("kit_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Price).HasColumnName("price").HasPrecision(10, 2);

            entity.HasOne(d => d.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Kit)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.KitId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(20);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("pending");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal").HasPrecision(10, 2);
            entity.Property(e => e.DeliveryFee).HasColumnName("delivery_fee").HasPrecision(10, 2).HasDefaultValue(0);
            entity.Property(e => e.Total).HasColumnName("total").HasPrecision(10, 2);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId);

            entity.HasOne(d => d.Address)
                .WithMany()
                .HasForeignKey(d => d.AddressId);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.KitId).HasColumnName("kit_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price").HasPrecision(10, 2);
            entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasPrecision(10, 2);

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Kit)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.KitId);
        });

        modelBuilder.Entity<DeliveryRoute>(entity =>
        {
            entity.ToTable("delivery_routes");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.District).HasColumnName("district").HasMaxLength(80);
            entity.Property(e => e.DeliveryStatus).HasColumnName("delivery_status").HasMaxLength(20).HasDefaultValue("assigned");
            entity.Property(e => e.GpsLat).HasColumnName("gps_lat").HasPrecision(10, 6);
            entity.Property(e => e.GpsLng).HasColumnName("gps_lng").HasPrecision(10, 6);
            entity.Property(e => e.DeliveredAt).HasColumnName("delivered_at");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.DeliveryRoutes)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Driver)
                .WithMany(p => p.DeliveryRoutes)
                .HasForeignKey(d => d.DriverId);
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.ToTable("payment_transactions");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Provider).HasColumnName("provider").HasMaxLength(40);
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id").HasMaxLength(200);
            entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(10, 2);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InventoryLog>(entity =>
        {
            entity.ToTable("inventory_logs");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Type).HasColumnName("type").HasMaxLength(20);
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("suppliers");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150);
            entity.Property(e => e.ContactName).HasColumnName("contact_name").HasMaxLength(150);
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(30);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(120);
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<SupplierOrder>(entity =>
        {
            entity.ToTable("supplier_orders");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.OrderDate).HasColumnName("order_date").HasDefaultValueSql("NOW()");
            entity.Property(e => e.Total).HasColumnName("total").HasPrecision(10, 2);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("pending");

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.SupplierId);
        });

        modelBuilder.Entity<SupplierOrderItem>(entity =>
        {
            entity.ToTable("supplier_order_items");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SupplierOrderId).HasColumnName("supplier_order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.CostPrice).HasColumnName("cost_price").HasPrecision(10, 2);
            entity.Property(e => e.Subtotal).HasColumnName("subtotal").HasPrecision(10, 2);

            entity.HasOne(d => d.SupplierOrder)
                .WithMany(p => p.SupplierOrderItems)
                .HasForeignKey(d => d.SupplierOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.SupplierOrderItems)
                .HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<SchoolList>(entity =>
        {
            entity.ToTable("school_lists");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SchoolName).HasColumnName("school_name").HasMaxLength(150);
            entity.Property(e => e.Grade).HasColumnName("grade").HasMaxLength(80);
            entity.Property(e => e.FileUrl).HasColumnName("file_url").HasMaxLength(255);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("pending_review");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(d => d.User)
                .WithMany(p => p.SchoolLists)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<SchoolListItem>(entity =>
        {
            entity.ToTable("school_list_items");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SchoolListId).HasColumnName("school_list_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SuggestedQuantity).HasColumnName("suggested_quantity");
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("available");

            entity.HasOne(d => d.SchoolList)
                .WithMany(p => p.SchoolListItems)
                .HasForeignKey(d => d.SchoolListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.SchoolListItems)
                .HasForeignKey(d => d.ProductId);
        });
    }
}