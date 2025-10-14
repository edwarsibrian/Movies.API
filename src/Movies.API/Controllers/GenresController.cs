using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Application.Commands;
using Movies.Application.DTOs;
using Movies.Application.Queries;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private const string CacheKey = "GenresCache";

        public GenresController(IOutputCacheStore outputCacheStore, IMediator mediator, IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetGenreById")]
        [OutputCache(Tags = new[] { CacheKey })]
        public async Task<ActionResult<GenreDTO>> Get(int id, CancellationToken cancellationToken)
        {
            var genre = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
            if(genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpGet]
        [OutputCache(Tags = new[] { CacheKey }, VaryByQueryKeys = new[] { "PageNumber", "RecordsByPage" })]
        public async Task<ActionResult<List<GenreDTO>>> Get([FromQuery] PaginationDTO pagination, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetGenresQuery(pagination), cancellationToken);
            HttpContext.Response.Headers.Append("TotalRecords", result.TotalRecords.ToString());
            return Ok(result.Items);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGenreDTO genreDTO, CancellationToken cancellationToken)
        {
            var command = new CreateGenreCommand(genreDTO.GenreName);
            var genre = await mediator.Send(command, cancellationToken);
            await outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return CreatedAtRoute("GetGenreById", new { id = genre.Id }, genre);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] GenreDTO genreDTO, CancellationToken cancellationToken)
        {
            if (id != genreDTO.Id)
            {
                return BadRequest("The id in the URL does not match the id in the body.");
            }
            var command = mapper.Map<UpdateGenreCommand>(genreDTO);
            var genre = await mediator.Send(command, cancellationToken);
            if (genre == null)
            {
                return NotFound();
            }
            await outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var rowsAffected = await mediator.Send(new DeleteGenreCommand(id), cancellationToken);
            if (rowsAffected == 0)
            {
                return NotFound();
            }
            await outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return NoContent();
        }

    }
}
