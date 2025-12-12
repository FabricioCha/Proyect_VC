using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilesArequipa.Application.Interfaces.Authentication;
using UtilesArequipa.Application.Interfaces.Persistence;
using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IWebHostEnvironment _env;

    public SeedController(IApplicationDbContext context, IPasswordHasher passwordHasher, IWebHostEnvironment env)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _env = env;
    }

    [HttpPost("init")]
    public async Task<IActionResult> Init()
    {
        if (!_env.IsDevelopment())
        {
            return NotFound();
        }

        // 1. Asegurar Admin
        var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@utiles.com");
        if (admin == null)
        {
            admin = new User
            {
                FirstName = "Admin",
                LastName = "System",
                Email = "admin@utiles.com",
                Role = "admin",
                CreatedAt = DateTime.UtcNow
            };
            await _context.Users.AddAsync(admin);
        }
        
        // Siempre actualizar password y asegurar rol
        admin.PasswordHash = _passwordHasher.HashPassword("Admin123!");
        admin.Role = "admin"; // Asegurar que sea admin
        admin.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(CancellationToken.None);

        if (await _context.Products.AnyAsync())
        {
            return Ok(new { message = "Admin restaurado. Productos ya existen." });
        }

        // 2. Crear Productos
        var p1 = new Product
        {
            Name = "Cuaderno A4 Stanford",
            Sku = "STF-A4-001",
            Price = 5.00m,
            Stock = 100,
            StockMin = 10,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CostPrice = 3.00m
        };

        var p2 = new Product
        {
            Name = "Caja Colores Faber 12u",
            Sku = "FAB-12-COL",
            Price = 12.00m,
            Stock = 50,
            StockMin = 5,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CostPrice = 8.00m
        };

        var p3 = new Product
        {
            Name = "Lápiz 2B Mongul",
            Sku = "MNG-2B-001",
            Price = 1.00m,
            Stock = 200,
            StockMin = 20,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CostPrice = 0.50m
        };

        _context.Products.AddRange(p1, p2, p3);

        // Guardamos para generar IDs
        await _context.SaveChangesAsync(CancellationToken.None);

        // 3. Crear Kit
        var kit = new Kit
        {
            Name = "Kit Básico Primaria",
            Description = "Kit esencial para inicio de clases",
            Price = 35.00m, // Precio especial
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Kits.Add(kit);
        await _context.SaveChangesAsync(CancellationToken.None);

        // 4. Asignar Items al Kit
        var kitItems = new List<KitItem>
        {
            new KitItem { KitId = kit.Id, ProductId = p1.Id, Quantity = 5 }, // 5 Cuadernos
            new KitItem { KitId = kit.Id, ProductId = p2.Id, Quantity = 1 }, // 1 Caja Colores
            new KitItem { KitId = kit.Id, ProductId = p3.Id, Quantity = 2 }  // 2 Lápices
        };

        _context.KitItems.AddRange(kitItems);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Ok(new 
        { 
            message = "Base de datos poblada correctamente.", 
            users = new { admin = "admin@utiles.com", customer = "cliente@utiles.com" },
            kit = kit.Name
        });
    }
}