using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo commandRepo;
    private readonly IMapper mapper;

    public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
    {
        this.commandRepo = commandRepo;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlatformsAsync()
    {
        var platforms = await commandRepo.GetAllPlatformsAsync();
        var result = mapper.Map<List<PlatformReadDto>>(platforms);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("===> Inbound Post # Command Service");
        return Ok("Inbound test from platforms controller in commands service.");
    }

}