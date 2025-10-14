using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.API.DTOs;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMediator mediator;

        public ActorsController(IOutputCacheStore outputCacheStore, IMediator mediator)
        {
            this.outputCacheStore = outputCacheStore;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateActorRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
