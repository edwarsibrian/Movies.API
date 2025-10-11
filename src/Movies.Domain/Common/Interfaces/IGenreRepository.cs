using Movies.Domain.Entities;

namespace Movies.Domain.Common.Interfaces
{
    public interface IGenreRepository
    {
        Task CreateAsync(Genre genre, CancellationToken cancellationToken);
        IQueryable<Genre> GetAll();
        Task<bool>UpdateAsync(Genre genre, CancellationToken cancellationToken);
        Task<int> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
