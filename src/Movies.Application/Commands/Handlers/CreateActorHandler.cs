using AutoMapper;
using MediatR;
using Movies.Application.DTOs;
using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.Commands.Handlers
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, ActorDTO>
    {
        private readonly IActorRepository repository;
        private readonly IMapper mapper;

        public CreateActorHandler(IActorRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ActorDTO> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {
            var actor = mapper.Map<Actor>(request);
            await repository.CreateAsync(actor, cancellationToken);
            return mapper.Map<ActorDTO>(actor);
        }
    }
}
