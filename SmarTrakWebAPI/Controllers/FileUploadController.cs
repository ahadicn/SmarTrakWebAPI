using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/files")]
public class FileUploadController : ControllerBase
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public FileUploadController(IConfiguration configuration)
    {
        string connectionString = configuration["AzureBlobStorage:ConnectionString"];
        _containerName = configuration["AzureBlobStorage:ContainerName"];
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    /// <summary>
    /// Uploads a file to Azure Blob Storage
    /// </summary>
    /// <param name="file">File to upload</param>
    /// <returns>Upload status</returns>
    [HttpPost("upload")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

            var blobClient = containerClient.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                TransferOptions = new StorageTransferOptions
                {
                    InitialTransferSize = 4 * 1024 * 1024,  // 4 MB chunks
                    MaximumTransferSize = 16 * 1024 * 1024  // 16 MB chunks
                }
            });

            return Ok(new
            {
                message = "File uploaded successfully",
                fileName = file.FileName,
                fileUrl = blobClient.Uri.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
