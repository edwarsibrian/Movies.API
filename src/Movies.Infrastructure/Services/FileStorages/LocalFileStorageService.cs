using Movies.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Infrastructure.Services.FileStorages
{
    public class LocalFileStorageService : IFileStorageService
    {
        public Task DeleteFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> EditFileAsync(string existingFilePath, Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFileAsync(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
