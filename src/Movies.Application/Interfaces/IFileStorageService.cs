namespace Movies.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream stream, string fileName, string container);
        Task DeleteFileAsync(string filePath, string container);
        Task<string> EditFileAsync(string existingFilePath, Stream stream, string fileName, string container);
    }
}
