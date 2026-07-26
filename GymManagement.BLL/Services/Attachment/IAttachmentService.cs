using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GymManagement.BLL.Services.Attachment
{
    public interface IAttachmentService
    {
        Task<string?> UploadFileAsync(Stream fileStream, string folderName, string fileName, CancellationToken ct = default);
        bool DeleteFile(string folderName, string fileName);

        (Stream fileStream, string contentType)? GetFile(string folderName, string fileName);

    }
}
