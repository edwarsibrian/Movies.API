using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Application.Commands;
using Movies.Application.DTOs;
using Movies.Application.Queries;
using Movies.Domain.Entities;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMediator mediator;

        private const string CacheKey = "GenresCache";

        public GenresController(IOutputCacheStore outputCacheStore, IMediator mediator)
        {
            this.outputCacheStore = outputCacheStore;
            this.mediator = mediator;
        }

        [HttpGet("{id:int}", Name = "GetGenreById")]
        [OutputCache(Tags = new[] { CacheKey })]
        public async Task<ActionResult<GenreDTO>> Get([FromQuery] int id, CancellationToken cancellationToken)
        {
            var genres = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
            return Ok(genres);
        }

        [HttpGet]
        [OutputCache(Tags = new[] { CacheKey })]
        public async Task<ActionResult<List<GenreDTO>>> Get(CancellationToken cancellationToken)
        {
            var genres = await mediator.Send(new GetGenresQuery(), cancellationToken);
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGenreCommand command, CancellationToken cancellationToken)
        {
            var genre = await mediator.Send(command, cancellationToken);
            await outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return CreatedAtRoute("GetGenreById", new { id = genre.Id }, genre);
        }

    }
}
