using AutoMapper;
using Northwind.Core.Models;
using Northwind.Web.ViewModels.Categories;
using Northwind.Web.ViewModels.Products;

namespace Northwind.Web.Utilities.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EditImageViewModel, CategoryDTO>().ReverseMap();
            CreateMap<CreateProductViewModel, ProductDTO>().ReverseMap();
            CreateMap<EditProductViewModel, ProductDTO>().ReverseMap();
        }
    }
}
