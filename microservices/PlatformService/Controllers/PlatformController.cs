using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Model;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepo platformRepo;
    private readonly IMapper mapper;
    private readonly IValidator<PlatformCreateDto> createValidator;

    public PlatformController(IPlatformRepo platformRepo, IMapper mapper, IValidator<PlatformCreateDto> createValidator)
    {
        this.platformRepo = platformRepo;
        this.mapper = mapper;
        this.createValidator = createValidator;
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

        return CreatedAtRoute(nameof(GetPlatformsByIdAsync), new { result.Id }, result);
    }
}