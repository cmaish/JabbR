using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using JabbR.ContentProviders;
using JabbR.Services;

namespace JabbR.UploadHandlers
{
    public class FileSystemUploadHandler : IUploadHandler
    {
        private readonly IApplicationSettings _settings;

        [ImportingConstructor]
        public FileSystemUploadHandler(IApplicationSettings settings)
        {
            _settings = settings;
        }

        public bool IsValid(string fileName, string contentType)
        {
            return !String.IsNullOrEmpty(_settings.FileSystemUploadStorageLocation) &&
                   ImageContentProvider.IsValidContentType(contentType);
        }

        public async Task<string> UploadFile(string fileName, string contentType, Stream stream)
        {
            // Randomize the filename
            string uploadFileName = Path.GetFileNameWithoutExtension(fileName) +
                                "_" +
                                Guid.NewGuid().ToString().Substring(0, 4) + Path.GetExtension(fileName);

            string fileSystemUploadLocation = Path.GetFullPath(_settings.FileSystemUploadStorageLocation);

            string fileSystemLocation = Path.GetFullPath(Path.Combine(fileSystemUploadLocation, uploadFileName));

            if (!string.Equals(fileSystemLocation, Path.Combine(_settings.FileSystemUploadStorageLocation, uploadFileName), StringComparison.Ordinal))
            {
                return null;
            }

            using (FileStream uploadStream = File.OpenWrite(fileSystemLocation))
            {
                await stream.CopyToAsync(uploadStream);
            }

            string chatUri = _settings.FileSystemUploadChatUrl;

            if (string.IsNullOrEmpty(chatUri))
            {
                // return the path
                return fileSystemLocation;
            }

            if (!chatUri.EndsWith("/"))
            {
                chatUri += '/';
            }

            return chatUri + uploadFileName;
        }
    }
}