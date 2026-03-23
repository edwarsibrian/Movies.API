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
        private readonly string _publicPath;
        private readonly Dictionary<string, string> _containers;
        private readonly IWebRootPathProvider _webRoot;

        public LocalFileStorageService(
            IOptions<FileStorageSettings> options, 
            IHostEnvironment env,
            IWebRootPathProvider webRoot)
        {
            var settings = options.Value;
            _webRoot = webRoot;
            _containers = settings.Containers;
            
            //if LocalPath is relative, convert to absolute path
            _basePath = Path.IsPathRooted(settings.LocalStoragePath) 
                ? settings.LocalStoragePath 
                : Path.Combine(env.ContentRootPath, settings.LocalStoragePath);

            _publicPath = Path.Combine(_webRoot.WebRootPath, settings.PublicStoragePath);

            Directory.CreateDirectory(_basePath);
            Directory.CreateDirectory(_publicPath);
        }

        public Task DeleteFileAsync(string filePath, string container)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                return Task.CompletedTask;
            }

            // Delete public file (wwwroot)
            var relativePath = filePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPublicPath = Path.Combine(_webRoot.WebRootPath, relativePath);

            if (File.Exists(fullPublicPath))
            {
                File.Delete(fullPublicPath);
            }

            // Delete physical file
            var directory = DirectoryHelper.GetDirectory(_basePath, _containers, container);
            var fileName = Path.GetFileName(fullPublicPath);
            var realFilePath = Path.Combine(directory, fileName);

            if (File.Exists(realFilePath))
            {
                File.Delete(realFilePath);
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

            // Physical file save
            using var fs=new FileStream(finalPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await stream.CopyToAsync(fs);

            // Create copy in public folder for web access
            var publicDirectory = Path.Combine(_publicPath, container);
            Directory.CreateDirectory(publicDirectory);

            var publicPath = Path.Combine(publicDirectory, fileName);
            File.Copy(finalPath, publicPath, overwrite: true);

            return $"/{Path.GetRelativePath(_webRoot.WebRootPath, publicPath).Replace("\\", "/")}";
        }       
                
    }
}
