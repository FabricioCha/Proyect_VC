using Microsoft.EntityFrameworkCore;
using UtilesArequipa.Domain.Entities;
using UtilesArequipa.Domain.Interfaces;
using UtilesArequipa.Infrastructure.Persistence.Contexts;

namespace UtilesArequipa.Infrastructure.Persistence.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Sku == sku);
    }
}