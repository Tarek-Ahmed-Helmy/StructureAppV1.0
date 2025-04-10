using AutoMapper;
using Domain.Models;
using Presentation.ViewModels;

namespace Presentation.Mappers;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeVM>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName)).ReverseMap()
            .ForMember(dest => dest.Department, opt => opt.Ignore());
    }
}
