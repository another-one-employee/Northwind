using AutoMapper;
using Northwind.Application.Models;
using Northwind.Domain.Entities;

namespace Northwind.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryEntity>().ReverseMap();
            CreateMap<Supplier, SupplierEntity>().ReverseMap();
            CreateMap<Product, ProductEntity>()
                .ForMember(p => p.SupplierEntity, s => s.MapFrom(p => p.Supplier))
                .ForMember(p => p.CategoryEntity, s => s.MapFrom(p => p.Category))
                .ReverseMap();
        }
    }
}
