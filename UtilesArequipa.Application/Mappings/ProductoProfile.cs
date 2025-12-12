using AutoMapper;
using UtilesArequipa.Application.Features.Productos.Commands;
using UtilesArequipa.Application.Features.Productos.DTOs;
using UtilesArequipa.Domain.Entities;

namespace UtilesArequipa.Application.Mappings;

public class ProductoProfile : Profile
{
    public ProductoProfile()
    {
        CreateMap<Product, ProductoDto>();
        CreateMap<CrearProductoCommand, Product>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.SKU));
    }
}
