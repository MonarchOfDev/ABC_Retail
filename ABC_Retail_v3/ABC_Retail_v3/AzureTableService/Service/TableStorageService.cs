using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ABC_Retail_v3.AzureTableService.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC_Retail_v3.AzureTableService.Service
{
    public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity, new()
    {
        private readonly CloudTable _cloudTable;

        public TableStorageService(string connectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = tableClient.GetTableReference(tableName);
            _cloudTable.CreateIfNotExistsAsync().Wait();
        }

        public async Task AddEntityAsync(T entity)
        {
            try
            {
                var insertOperation = TableOperation.Insert(entity);
                await _cloudTable.ExecuteAsync(insertOperation);
            }
            catch (StorageException ex)
            {
                Console.WriteLine($"StorageException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetEntityAsync(string partitionKey, string rowKey)
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var result = await _cloudTable.ExecuteAsync(retrieveOperation);
                return result.Result as T;
            }
            catch (StorageException ex)
            {
                Console.WriteLine($"StorageException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateEntityAsync(T entity)
        {
            try
            {
                var replaceOperation = TableOperation.Replace(entity);
                await _cloudTable.ExecuteAsync(replaceOperation);
            }
            catch (StorageException ex)
            {
                Console.WriteLine($"StorageException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteEntityAsync(string partitionKey, string rowKey)
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var result = await _cloudTable.ExecuteAsync(retrieveOperation);
                var deleteEntity = result.Result as T;

                if (deleteEntity != null)
                {
                    var deleteOperation = TableOperation.Delete(deleteEntity);
                    await _cloudTable.ExecuteAsync(deleteOperation);
                }
            }
            catch (StorageException ex)
            {
                Console.WriteLine($"StorageException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<List<T>> GetAllEntitiesAsync()
        {
            var entities = new List<T>();
            try
            {
                TableQuery<T> query = new TableQuery<T>();
                TableContinuationToken token = null;

                do
                {
                    var segment = await _cloudTable.ExecuteQuerySegmentedAsync(query, token);
                    token = segment.ContinuationToken;
                    entities.AddRange(segment.Results);
                } while (token != null);
            }
            catch (StorageException ex)
            {
                Console.WriteLine($"StorageException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }

            return entities;
        }
    }
}
