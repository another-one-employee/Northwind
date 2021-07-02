using AutoMapper;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile() => CreateMap<Models.Product, ProductDTO>().ReverseMap();
    }
}
