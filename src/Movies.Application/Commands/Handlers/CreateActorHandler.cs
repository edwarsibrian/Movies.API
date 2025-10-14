using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Commands.Handlers
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, int>
    {
        public CreateActorHandler()
        {
        }

        public Task<int> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
