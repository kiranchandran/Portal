using AutoMapper;
using CompanyName.Data.Entity;
using CompanyName.Model.Models;

namespace CompanyName.Services.Mapping
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<Department, DepartmentModel>();
        }
    }
}
