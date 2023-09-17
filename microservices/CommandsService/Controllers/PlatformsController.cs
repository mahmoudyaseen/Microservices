using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {
        
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("===> Inbound Post # Command Service");
        return Ok("Inbound test from platforms controller in commands service.");
    }

}