using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Domain.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku);
}