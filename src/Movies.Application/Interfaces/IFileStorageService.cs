namespace Movies.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream stream, string fileName);
        Task DeleteFileAsync(string filePath);
        Task<string> EditFileAsync(string existingFilePath, Stream stream, string fileName);
    }
}
