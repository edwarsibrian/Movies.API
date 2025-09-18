using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Application.Commands;
using Movies.Domain.Entities;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMediator mediator;

        public GenresController(IOutputCacheStore outputCacheStore, IMediator mediator)
        {
            this.outputCacheStore = outputCacheStore;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGenreCommand command, CancellationToken cancellationToken)
        {
            var genre = await mediator.Send(command, cancellationToken);
            await outputCacheStore.EvictByTagAsync("GenresCache", cancellationToken);
            return CreatedAtAction(nameof(Post), new { id = genre.Id }, genre);
        }

    }
}
