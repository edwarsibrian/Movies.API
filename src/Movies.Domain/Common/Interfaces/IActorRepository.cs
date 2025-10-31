using Movies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Common.Interfaces
{
    public interface IActorRepository
    {
        Task CreateAsync(Actor actor, CancellationToken cancellationToken);
    }
}
