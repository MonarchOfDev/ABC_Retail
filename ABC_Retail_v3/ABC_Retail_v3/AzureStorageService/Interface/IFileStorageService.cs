namespace ABC_Retail_v3.AzureStorageService.Interface
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(string fileName, Stream fileStream);
    }
}
