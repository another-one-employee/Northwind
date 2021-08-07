using AutoMapper;
using Northwind.Core.Entities;
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
            CreateMap<EditImageViewModel, Category>().ReverseMap();
            CreateMap<CreateProductViewModel, Product>().ReverseMap();
            CreateMap<EditProductViewModel, Product>().ReverseMap();

            CreateMap<Product, ProductModel>();
            CreateMap<Supplier, SupplierModel>();
            CreateMap<Category, CategoryModel>();

            CreateMap<CreateProductModel, Product>();
            CreateMap<UpdateProductModel, Product>();
        }
    }
}
