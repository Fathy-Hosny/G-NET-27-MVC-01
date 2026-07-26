using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace GymManagement.BLL.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment _env;

        public AttachmentService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool DeleteFile(string folderName, string fileName)
        {

            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
                if (!File.Exists(filePath)) return false;

                File.Delete(filePath);

                return true;

            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        public (Stream fileStream, string contentType)? GetFile(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName)) return null;
            var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(filePath)) return null;
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(filePath).ToLower();
            var contentType = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream" //Binary file
            };
            return (fileStream, contentType);

        }

        public async Task<string?> UploadFileAsync(Stream fileStream, string folderName, string fileName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null;
            if (fileStream.Length > _maxFileSize) return null;

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension)) return null;

            var uploadsFolder = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(uploadsFolder);
            var storedFileName = $"{Guid.NewGuid()}{fileName}";
            var filePath = Path.Combine(uploadsFolder, storedFileName);
            try
            {
                using var fr = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fr);
                return storedFileName;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return null;
            }
        }
    }
}