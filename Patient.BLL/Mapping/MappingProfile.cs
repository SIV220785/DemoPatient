using AutoMapper;
using Patient.BLL.Models;
using Patient.DAL.Entities;

namespace Patient.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping from Entity to DTO
        CreateMap<Given, GivenDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<PatientProfile, PatientProfileDto>()
            .ForMember(dest => dest.PatientDetail, opt => opt.MapFrom(src => src.PatientDetail))
            .ForPath(dest => dest.BirthDate, opt => opt.MapFrom(src => src.RecordDate))
            .ForPath(dest => dest.IsActive, opt => opt.MapFrom(src => src.Active.IsActive))
            .ForPath(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Type));

        CreateMap<PatientDetail, PatientDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Use, opt => opt.MapFrom(src => src.Use))
            .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.Family))
            .ForMember(dest => dest.Givens, opt => opt.MapFrom(src => src.Givens.Select(g => g.Name)));

        // Mapping from DTO to Entity
        CreateMap<PatientProfileDto, PatientProfile>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.Gender.Type, opt => opt.MapFrom(src => src.Gender))
            .ForPath(dest => dest.Active.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<PatientDetailsDto, PatientDetail>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Use, opt => opt.MapFrom(src => src.Use))
            .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.Family))
            .ForMember(dest => dest.Givens, opt => opt.MapFrom(src => src.Givens.Select(name => new Given { Name = name })));

        CreateMap<GivenDto, Given>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
