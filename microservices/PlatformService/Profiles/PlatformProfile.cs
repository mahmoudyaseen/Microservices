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
    }
}