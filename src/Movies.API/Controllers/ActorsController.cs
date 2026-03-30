using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.API.DTOs;
using Movies.Application.Commands;
using Movies.Application.Contracts.Messaging;
using Movies.Application.Interfaces;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly IMediator _mediator;
        private readonly IMessagePublisher _publisher;
        private readonly string _containerName = "Actors";
        private const string CacheKey = "ActorsCache";

        public ActorsController(IOutputCacheStore outputCacheStore, IMediator mediator, IMessagePublisher publisher)
        {
            _outputCacheStore = outputCacheStore;
            _mediator = mediator;
            _publisher = publisher;
        }

        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task Get(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateActorRequest request, CancellationToken cancellationToken)
        {
            string tempFilePath = string.Empty;

            if(request.Picture is not null)
            {
                var fileName = $"{Guid.NewGuid()}_{request.Picture.FileName}";
                tempFilePath = Path.Combine("temp", fileName);

                Directory.CreateDirectory("temp");
                
                using var stream = new FileStream(tempFilePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream, cancellationToken);
            }

            var command = new CreateActorCommand(request.ActorName, request.BirthDate, null);
            var actor = await _mediator.Send(command, cancellationToken);

            if(!string.IsNullOrEmpty(tempFilePath))
            {
                await _publisher.PublishAsync(new ActorFileMessage
                {
                    ActorId = actor.Id,
                    FilePath = tempFilePath,
                    FileName = Path.GetFileName(tempFilePath),
                    ContainerName = _containerName
                }, QueueNames.ActorImageUploadQueue);
            }

            await _outputCacheStore.EvictByTagAsync(CacheKey, cancellationToken);
            return CreatedAtRoute("GetActorById", new { id = actor.Id }, actor);
        }
    }
}
