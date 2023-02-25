using AutoMapper;
using CompanyName.Data.Entity;
using CompanyName.Model.Models;

namespace CompanyName.Services.Mapping
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeModel>();

            CreateMap<EmployeeModel, Employee>()
                .ForMember(f => f.Id, m => m.Ignore());

            CreateMap<SaveEmployeeRequest, Employee>()
                .ForMember(f => f.Id, m => m.Ignore());
        }
    }
}
