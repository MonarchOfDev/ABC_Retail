using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC_Retail_v3.AzureTableService.Interface
{
    public interface ITableStorageService<T> where T : class, ITableEntity, new()
    {
        Task AddEntityAsync(T entity);
        Task<T> GetEntityAsync(string partitionKey, string rowKey);
        Task UpdateEntityAsync(T entity);
        Task DeleteEntityAsync(string partitionKey, string rowKey);
        Task<List<T>> GetAllEntitiesAsync();
    }
}
