using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.API.DTOs;
using Movies.Application.Commands;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMediator mediator;
        private const string CacheKey = "ActorsCache";

        public ActorsController(IOutputCacheStore outputCacheStore, IMediator mediator)
        {
            this.outputCacheStore = outputCacheStore;
            this.mediator = mediator;
        }

        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task Get(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateActorRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateActorCommand(request.ActorName, request.BirthDate, string.Empty);
            var actor = await mediator.Send(command, cancellationToken);
            await outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return CreatedAtRoute("GetActorById", new { id = actor }, actor);
        }
    }
}
