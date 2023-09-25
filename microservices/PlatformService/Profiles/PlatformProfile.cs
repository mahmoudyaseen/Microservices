using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Model;

namespace PlatformService.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        
        CreateMap<PlatformCreateDto, Platform>();

        CreateMap<PlatformReadDto, PlatformPublishDto>();
        
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(des => des.PlatformId, opt => opt.MapFrom(src => src.Id))
            .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(des => des.Publisher, opt => opt.MapFrom(src => src.Publisher));
    }
}