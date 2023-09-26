using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles;

public class CommandProfile : Profile
{
    public CommandProfile()
    {
        CreateMap<Platform, PlatformReadDto>();

        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();

        CreateMap<PlatformPublishDto, Platform>()
            .ForMember(des => des.ExternalId, opt => opt.MapFrom(src => src.Id));

        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(des => des.ExternalId, opt => opt.MapFrom(src => src.PlatformId));
    }
}