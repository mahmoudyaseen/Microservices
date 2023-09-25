using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlaform.GrpcPlaformBase
{
    private readonly IPlatformRepo platformRepo;
    private readonly IMapper mapper;

    public GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper)
    {
        this.platformRepo = platformRepo;
        this.mapper = mapper;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = await platformRepo.GetAllPlatformsAsync();

        foreach (var platform in platforms)
        {   
            var dto = mapper.Map<GrpcPlatformModel>(platform);
            response.Platform.Add(dto);
        }

        return response;
    }
}