namespace ABC_Retail_v3.AzureBlobService.Interface
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string fileName, Stream fileStream);
        Task<Stream> DownloadFileAsync(string fileName);
    }
}
