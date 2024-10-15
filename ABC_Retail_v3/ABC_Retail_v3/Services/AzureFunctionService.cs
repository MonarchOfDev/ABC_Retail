using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ABC_Retail_v3.Models;

namespace ABC_Retail_v3.Services
{
    public class AzureFunctionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _storeUserFunctionUrl;
        private readonly string _uploadBlobFunctionUrl;
        private readonly string _addToQueueFunctionUrl;
        private readonly string _uploadFileFunctionUrl;
        private readonly string _getUserFunctionUrl;

        public AzureFunctionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _storeUserFunctionUrl = configuration["AzureFunctions:StoreUser"];
            _uploadBlobFunctionUrl = configuration["AzureFunctions:UploadBlob"];
            _addToQueueFunctionUrl = configuration["AzureFunctions:AddToQueue"];
            _uploadFileFunctionUrl = configuration["AzureFunctions:UploadFile"];
            _getUserFunctionUrl = configuration["AzureFunctions:GetUser"];
        }

        // Store User in Azure Table Storage via Azure Function
        public async Task<string> StoreUserAsync(CraftUsers user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_storeUserFunctionUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Upload File to Azure Blob Storage via Azure Function
        public async Task<string> UploadBlobAsync(IFormFile file)
        {
            var form = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            form.Add(fileContent, "file", file.FileName);
            var response = await _httpClient.PostAsync(_uploadBlobFunctionUrl, form);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Add Transaction Message to Azure Queue via Azure Function
        public async Task<string> AddToQueueAsync(object transaction)
        {
            var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_addToQueueFunctionUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Upload File to Azure Files via Azure Function
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var form = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            form.Add(fileContent, "file", file.FileName);
            var response = await _httpClient.PostAsync(_uploadFileFunctionUrl, form);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Retrieve User from Azure Table Storage via Azure Function
        public async Task<CraftUsers> GetUserAsync(string partitionKey, string rowKey)
        {
            // Assuming the GetUser Azure Function accepts partitionKey and rowKey as query parameters
            var requestUrl = $"{_getUserFunctionUrl}?partitionKey={partitionKey}&rowKey={rowKey}";
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var userJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CraftUsers>(userJson);
        }
    }
}
