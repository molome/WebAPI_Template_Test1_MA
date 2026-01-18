using Azure.Storage.Blobs;

namespace WebAPI_Template_Test1_MA.Services.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadBlob(IFormFile formFile, string imageName);
        Task<string> GetBlobUrl(string imageName);
        Task RemoveBlob(string imageName);
    }
}