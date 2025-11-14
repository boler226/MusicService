using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Queries.GetPlaylist;

namespace MusicService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController(IMediator _mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPlaylistQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}