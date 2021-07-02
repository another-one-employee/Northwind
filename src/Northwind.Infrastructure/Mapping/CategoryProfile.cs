using AutoMapper;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() => CreateMap<Models.Category, CategoryDTO>().ReverseMap();
    }
}
