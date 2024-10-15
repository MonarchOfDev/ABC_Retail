using ABC_Retail_v3.AzureStorageService.Interface;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;

namespace ABC_Retail_v3.AzureStorageService.Service;

public class FileStorageService : IFileStorageService
{
    private readonly ShareClient _shareClient;

    public FileStorageService(string connectionString)
    {
        var fileServiceClient = new ShareServiceClient(connectionString);
        //create a blob storage if it doesn't exist
        _shareClient = fileServiceClient.GetShareClient("abcfiles");
        _shareClient.CreateIfNotExists();
    }
    //upload file to blob storage container
    public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
    {
        try
        {
            var fileClient = _shareClient.GetRootDirectoryClient().GetFileClient("abddir/" + fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, fileStream.Length), fileStream);
            return fileClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            Console.WriteLine($"Error uploading file to File storage: {ex.Message}");
            throw; // Optionally rethrow the exception if you want it to propagate
        }

    }
}