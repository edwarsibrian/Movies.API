namespace Movies.Application.Interfaces
{
    public interface ICacheService
    {
        Task EvictByTagAsync(string tag, CancellationToken cancellationToken = default);
    }
}
