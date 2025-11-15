using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Movies.Application.Interfaces;
using Movies.Infrastructure.Helpers;
using Movies.Infrastructure.Settings;


namespace Movies.Infrastructure.Services.FileStorages
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly Dictionary<string, string> _containers;

        public LocalFileStorageService(IOptions<FileStorageSettings> options, IHostEnvironment env)
        {
            var settings = options.Value;
            _containers = settings.Containers;
            
            //if LocalPath is relative, convert to absolute path
            _basePath = Path.IsPathRooted(settings.LocalStoragePath) 
                ? settings.LocalStoragePath 
                : Path.Combine(env.ContentRootPath, settings.LocalStoragePath);
            Directory.CreateDirectory(_basePath);
        }

        public Task DeleteFileAsync(string filePath, string container)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public async Task<string> EditFileAsync(string existingFilePath, Stream stream, string fileName, string container)
        {
            try
            {
                if(File.Exists(existingFilePath))
                {
                    File.Delete(existingFilePath);
                }
            }
            catch
            {
                //Log exception if needed
            }
            return await SaveFileAsync(stream, fileName, container);
        }

        public async Task<string> SaveFileAsync(Stream stream, string fileName, string container)
        {
            var directory = DirectoryHelper.GetDirectory(_basePath, _containers, container);
            var finalPath = Path.Combine(directory, fileName);

            //If stream is not at the beginning, reset position
            stream.Position = 0;

            using var fs=new FileStream(finalPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await stream.CopyToAsync(fs);

            return finalPath;
        }       
                
    }
}
