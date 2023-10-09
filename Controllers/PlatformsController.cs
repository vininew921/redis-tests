using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repo;

    public PlatformsController(IPlatformRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("{id}", Name = nameof(GetPlatformById))]
    public IActionResult GetPlatformById([FromRoute] string id)
    {
        Platform? platform = _repo.GetPlatformById(id);

        return platform == null
            ? NotFound()
            : Ok(platform);
    }

    [HttpGet]
    public IActionResult GetAllPlatforms()
    {
        return Ok(_repo.GetAllPlatforms());
    }

    [HttpPost]
    public IActionResult CreatePlatform([FromBody] Platform platform)
    {
        _repo.CreatePlatform(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platform);
    }
}

