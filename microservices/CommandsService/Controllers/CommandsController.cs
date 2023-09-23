using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo commandRepo;
    private readonly IMapper mapper;
    private readonly IValidator<CommandCreateDto> createCommandValidator;

    public CommandsController(
        ICommandRepo commandRepo,
        IMapper mapper,
        IValidator<CommandCreateDto> createCommandValidator
        )
    {
        this.commandRepo = commandRepo;
        this.mapper = mapper;
        this.createCommandValidator = createCommandValidator;
    }


    [HttpGet]
    public async Task<IActionResult> GetCommandsForPlatformAsync(int platformId)
    {
        var isPlatformExist = await commandRepo.IsPlatformExistsAsync(platformId);
        if (!isPlatformExist)
            return NotFound("This platform is not found!");

        var commands = await commandRepo.GetCommandsForPlatformAsync(platformId);
        var result = mapper.Map<List<CommandReadDto>>(commands);
        return Ok(result);
    }

    [HttpGet("{commandId}", Name = nameof(GetCommandAsync))]
    public async Task<IActionResult> GetCommandAsync(int platformId, int commandId)
    {
        var isPlatformExist = await commandRepo.IsPlatformExistsAsync(platformId);
        if (!isPlatformExist)
            return NotFound("This platform is not found!");

        var command = await commandRepo.GetCommandAsync(platformId, commandId);

        if (command is null)
            return NotFound("This command is not found!");

        var result = mapper.Map<CommandReadDto>(command);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandAsync(int platformId, CommandCreateDto createDto)
    {
        var isPlatformExist = await commandRepo.IsPlatformExistsAsync(platformId);
        if (!isPlatformExist)
            return NotFound("This platform is not found!");

        var validationResult = createCommandValidator.Validate(createDto);
        if (!validationResult.IsValid)
            return StatusCode(400, validationResult.Errors);

        var command = mapper.Map<Command>(createDto);
        commandRepo.CreateCommand(platformId, command);

        try
        {
            await commandRepo.SaveChangesAsync();
            var readDto = mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandAsync), new { readDto.PlatformId, CommandId = readDto.Id }, readDto);
        }
        catch (Exception ex)
        {
            return BadRequest($" Can't create command {ex.Message}.");
        }
    }
}