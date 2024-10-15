namespace ABC_Retail_v3.AzureQueueService.Interface
{
    public interface IQueueStorageService
    {
        Task SendMessageAsync(string message);
    }
}
