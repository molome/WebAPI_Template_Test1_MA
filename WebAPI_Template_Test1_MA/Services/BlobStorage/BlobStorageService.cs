using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.VisualBasic;

namespace WebAPI_Template_Test1_MA.Services.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadBlob(IFormFile formFile, string imageName)
        {
            var blobName = $"{imageName}{Path.GetExtension(formFile.FileName)}";
            var containerClient = await GetBlobContainerClientAsync();
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var client = await containerClient.UploadBlobAsync(blobName, memoryStream);
                return blobName;
            }
        }

        public async Task<string> GetBlobUrl(string imageName)
        {
            var containerClient = await GetBlobContainerClientAsync();
            var blob = containerClient.GetBlobClient(imageName);

            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
            return blob.GenerateSasUri(blobSasBuilder).ToString();
        }

        public async Task RemoveBlob(string imageName)
        {
            var containerClient = await GetBlobContainerClientAsync();
            var blob = containerClient.GetBlobClient(imageName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        private async Task<BlobContainerClient> GetBlobContainerClientAsync()
        {
            try
            {
                BlobContainerClient containerClient = new BlobContainerClient(
                _configuration.GetConnectionString("AzureStorageAccountConnection"), _configuration["StorageAccount:ContainerName"]);
                await containerClient.CreateIfNotExistsAsync();
                return containerClient;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
