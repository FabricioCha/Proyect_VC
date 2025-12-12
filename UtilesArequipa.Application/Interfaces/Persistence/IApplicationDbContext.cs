using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Application.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Address> Addresses { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<ProductImage> ProductImages { get; set; }
    DbSet<Kit> Kits { get; set; }
    DbSet<KitItem> KitItems { get; set; }
    DbSet<Cart> Carts { get; set; }
    DbSet<CartItem> CartItems { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<DeliveryRoute> DeliveryRoutes { get; set; }
    DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    DbSet<InventoryLog> InventoryLogs { get; set; }
    DbSet<Supplier> Suppliers { get; set; }
    DbSet<SupplierOrder> SupplierOrders { get; set; }
    DbSet<SupplierOrderItem> SupplierOrderItems { get; set; }
    DbSet<SchoolList> SchoolLists { get; set; }
    DbSet<SchoolListItem> SchoolListItems { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
