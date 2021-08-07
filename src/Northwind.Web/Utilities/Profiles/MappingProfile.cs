using AutoMapper;
using Northwind.Domain.Entities;
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
            CreateMap<EditImageViewModel, CategoryEntity>().ReverseMap();
            CreateMap<CreateProductViewModel, ProductEntity>().ReverseMap();
            CreateMap<EditProductViewModel, ProductEntity>().ReverseMap();

            CreateMap<ProductEntity, ProductModel>();
            CreateMap<SupplierEntity, SupplierModel>();
            CreateMap<CategoryEntity, CategoryModel>();

            CreateMap<CreateProductModel, ProductEntity>();
            CreateMap<UpdateProductModel, ProductEntity>();
        }
    }
}
