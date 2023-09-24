using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Model;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo platformRepo;
    private readonly IMapper mapper;
    private readonly IValidator<PlatformCreateDto> createValidator;
    private readonly ICommandDataClient commandDataClient;
    private readonly IMessageBusClient messageBusClient;

    public PlatformsController(
        IPlatformRepo platformRepo,
        IMapper mapper,
        IValidator<PlatformCreateDto> createValidator,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient
        )
    {
        this.platformRepo = platformRepo;
        this.mapper = mapper;
        this.createValidator = createValidator;
        this.commandDataClient = commandDataClient;
        this.messageBusClient = messageBusClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlatformsAsync()
    {
        var platforms = await platformRepo.GetAllPlatformsAsync();
        var dtos = mapper.Map<List<PlatformReadDto>>(platforms);
        return Ok(dtos);
    }

    [HttpGet("{id}", Name = nameof(GetPlatformsByIdAsync))]
    public async Task<IActionResult> GetPlatformsByIdAsync(int id)
    {
        var platform = await platformRepo.GetPlatformByIdAsync(id);

        if (platform is null)
            return NotFound();

        var dto = mapper.Map<PlatformReadDto>(platform);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlatformAsync(PlatformCreateDto dto)
    {
        var validationResult = createValidator.Validate(dto);
        if (!validationResult.IsValid)
            return StatusCode(400, validationResult.Errors);

        var platform = mapper.Map<Platform>(dto);
        platformRepo.CreatePlatform(platform);
        await platformRepo.SaveChangesAsync();

        var result = mapper.Map<PlatformReadDto>(platform);

        // send sync message
        try
        {
            await commandDataClient.SendPlatformToCommand(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"===> can't send creation syncronosly: {ex.Message}");
        }

        // send async message
        try
        {
            var publishPlatformDto = mapper.Map<PlatformPublishDto>(result);
            publishPlatformDto.Event = "Platform_Published";
            messageBusClient.PublishNewPlatform(publishPlatformDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"===> can't send creation asyncronosly: {ex.Message}");
        }

        return CreatedAtRoute(nameof(GetPlatformsByIdAsync), new { result.Id }, result);
    }
}