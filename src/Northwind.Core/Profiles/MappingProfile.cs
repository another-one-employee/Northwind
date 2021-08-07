using AutoMapper;
using Northwind.Core.Entities;

namespace Northwind.Core.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Category, Category>().ReverseMap();
            CreateMap<Models.Supplier, Supplier>().ReverseMap();
            CreateMap<Models.Product, Product>().ReverseMap();
        }
    }
}
