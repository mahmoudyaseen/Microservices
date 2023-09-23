using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles;

public class CommandProfile : Profile
{
    public CommandProfile()
    {
        CreateMap<Platform, PlatformReadDto>();

        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
    }
}