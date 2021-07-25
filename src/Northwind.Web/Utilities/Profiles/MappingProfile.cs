using AutoMapper;
using Northwind.Core.Models;
using Northwind.Web.ViewModels.Api;
using Northwind.Web.ViewModels.Api.Categories;
using Northwind.Web.ViewModels.Api.Products;
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

            CreateMap<ProductDTO, ProductModel>();
            CreateMap<SupplierDTO, SupplierModel>();
            CreateMap<CategoryDTO, CategoryModel>();

            CreateMap<CreateProductModel, ProductDTO>();
            CreateMap<UpdateProductModel, ProductDTO>();
        }
    }
}
