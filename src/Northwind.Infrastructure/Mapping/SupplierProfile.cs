using AutoMapper;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Mapping
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile() => CreateMap<Models.Supplier, SupplierDTO>().ReverseMap();
    }
}
