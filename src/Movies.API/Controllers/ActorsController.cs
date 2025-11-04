using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.API.DTOs;
using Movies.Application.Commands;
using Movies.Application.Interfaces;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMediator mediator;
        private readonly IFileStorageService fileStorageService;
        private readonly string _containerName = "Actors";
        private const string CacheKey = "ActorsCache";

        public ActorsController(IOutputCacheStore outputCacheStore, IMediator mediator, IFileStorageService fileStorageService)
        {
            this.outputCacheStore = outputCacheStore;
            this.mediator = mediator;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task Get(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateActorRequest request, CancellationToken cancellationToken)
        {
            string pictureUrl = string.Empty;
            if(request.Picture is not null)
            {
                var stream = request.Picture.OpenReadStream();
                pictureUrl = await fileStorageService.SaveFileAsync(stream, request.Picture.FileName, _containerName);
            }

            var command = new CreateActorCommand(request.ActorName, request.BirthDate, pictureUrl);
            var actor = await mediator.Send(command, cancellationToken);
            await outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return CreatedAtRoute("GetActorById", new { id = actor.Id }, actor);
        }
    }
}
