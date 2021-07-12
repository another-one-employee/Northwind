using AutoMapper;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() => CreateMap<Models.Category, CategoryDTO>().ReverseMap();
    }
}
