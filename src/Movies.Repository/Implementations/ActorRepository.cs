using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;
using Movies.Repository.Context;

namespace Movies.Repository.Implementations
{
    public class ActorRepository : IActorRepository
    {
        private readonly APIMovieDbContext dbContext;

        public ActorRepository(APIMovieDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Actor actor, CancellationToken cancellationToken)
        {
            await dbContext.Actors.AddAsync(actor, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
