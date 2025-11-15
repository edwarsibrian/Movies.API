using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Movies.Application.Interfaces;
using Movies.Infrastructure.Settings;

namespace Movies.Infrastructure.Services.FileStorages
{
    public class AzureFileStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly Dictionary<string, string> _containers;

        public AzureFileStorageService(IOptions<FileStorageSettings> options)
        {
            var settings = options.Value;
            _blobServiceClient = new BlobServiceClient(settings.ConnectionString);
            _containers = settings.Containers;
        }

        public async Task DeleteFileAsync(string filePath, string container)
        {
            var containerName = GetContainerName(container);
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var fileName = Path.GetFileName(filePath);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<string> EditFileAsync(string existingFilePath, Stream stream, string fileName, string container)
        {
            await DeleteFileAsync(existingFilePath, container);
            return await SaveFileAsync(stream, fileName, container);
        }

        public async Task<string> SaveFileAsync(Stream stream, string fileName, string container)
        {
            var containerName = GetContainerName(container);
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            
            var extension = Path.GetExtension(fileName);
            var blobName = $"{Guid.NewGuid()}{extension}";
            var blobClient = containerClient.GetBlobClient(blobName);
            stream.Position = 0;
            await blobClient.UploadAsync(stream, overwrite: true);
            
            return blobClient.Uri.ToString();
        }

        public async Task<string> UploadFromFilePathAsync(string localFilePath, string fileName, string container)
        {
            using var fs = File.OpenRead(localFilePath);
            return await SaveFileAsync(fs, fileName, container);
        }

        private string GetContainerName(string containerKey)
        {
            if (_containers.TryGetValue(containerKey, out var containerName))
            {
                return containerName;
            }
            throw new ArgumentException($"Container with key '{containerKey}' not configured.");
        }
    }
}
