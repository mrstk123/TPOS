using AutoMapper;
using TPOS.Api.Dtos.Request;
using TPOS.Api.Dtos.Response;
using TPOS.Domain.Entities.Generated;
using Object = TPOS.Domain.Entities.Generated.Object;

namespace TPOS.Api.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ReverseMap() will use the same configuration for both directions.
            // meaning that if a property is ignored, ReverseMap() ignore that property in other direction


            // Sample to get UserName of CreatedBy
            // CreateMap<Customer, CustomerResponseDto>()
            // .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser.UserName)); 

            CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName).Distinct().ToList()));

            // Customers
            // CustomerDto has Base Columns, so need to ignore Base Columns in mapping. Other Dto does not use Base Columns
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<CustomerRequestDto, Customer>()
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            CreateMap<CustomerRequestDto, ContactInfo>()
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
            CreateMap<ContactInfo, CustomerResponseDto>()
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Active, opt => opt.Ignore());

            // Employee
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(dest => dest.DeparmentName, opt => opt.MapFrom(src => src.Department.ObjKey))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.ObjKey));
            CreateMap<ContactInfo, EmployeeResponseDto>();
            CreateMap<Branch, EmployeeResponseDto>();
            CreateMap<EmployeeRequestDto, Employee>();
            CreateMap<EmployeeRequestDto, ContactInfo>();

            // Branch
            CreateMap<Branch, BranchResponseDto>();
            CreateMap<ContactInfo, BranchResponseDto>();
            CreateMap<Company, BranchResponseDto>();
            CreateMap<BranchRequestDto, ContactInfo>();

        }
    }
}
