using AutoMapper;
using Northwind.Core.Models;
using Northwind.Web.ViewModels.Categories;

namespace Northwind.Web.Utilities.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EditImageViewModel, CategoryDTO>().ReverseMap();
        }
    }
}
