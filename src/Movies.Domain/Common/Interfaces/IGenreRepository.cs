using Movies.Domain.Entities;

namespace Movies.Domain.Common.Interfaces
{
    public interface IGenreRepository
    {
        Task CreateAsync(Genre genre, CancellationToken cancellationToken);
    }
}
