using Movies.Application.Interfaces;

namespace Movies.Infrastructure.Services.FileStorages
{
    public class FallbackFileStorageService : IFileStorageService
    {
        private readonly AzureFileStorageService _azure;
        private readonly LocalFileStorageService _local;

        public FallbackFileStorageService(AzureFileStorageService azure, LocalFileStorageService local)
        {
            _azure = azure;
            _local = local;
        }

        public async Task DeleteFileAsync(string filePath, string container)
        {
            try
            {
                await _azure.DeleteFileAsync(filePath, container);
            }
            catch
            {
                await _local.DeleteFileAsync(filePath, container);
            }
        }

        public async Task<string> EditFileAsync(string existingFilePath, Stream stream, string fileName, string container)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            
            memoryStream.Position = 0;

            try
            {
                return await _azure.EditFileAsync(existingFilePath, memoryStream, fileName, container);
            }
            catch
            {
                memoryStream.Position = 0;
                return await _local.EditFileAsync(existingFilePath, stream, fileName, container);
            }
        }

        public async Task<string> SaveFileAsync(Stream stream, string fileName, string container)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            
            memoryStream.Position = 0;

            // Try to save to Azure first, if it fails, save to local
            try
            {              
                return await _azure.SaveFileAsync(memoryStream, fileName, container);
            }
            catch
            {
                memoryStream.Position = 0;
                return await _local.SaveFileAsync(stream, fileName, container);
            }
        }
    }
}
