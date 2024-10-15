using ABC_Retail_v3.AzureBlobService.Interface;
using Azure.Storage.Blobs;

namespace ABC_Retail_v3.AzureBlobService.Service;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _blobContainerClient;

    public BlobStorageService(string connectionString)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient("productimages");
        _blobContainerClient.CreateIfNotExists();
    }

    //upload file to blob storage container
    public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
    {
        try
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, true);
            return blobClient.Uri.ToString(); //return URL of uploaded file
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file to blob storage: {ex.Message}");
            throw;
        }

    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        try
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            var download = await blobClient.DownloadAsync();
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file from blob storage: {ex.Message}");
            throw;
        }
    }
}