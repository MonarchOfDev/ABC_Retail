using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Newtonsoft.Json;
using System.Linq;

namespace ABCRetail_Functions_v9
{
    public static class SendFileToAzureFiles
    {
        [FunctionName("SendFileToAzureFiles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploadFile")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SendFileToAzureFiles function processed a request.");

            if (!req.Form.Files.Any())
            {
                return new BadRequestObjectResult("No file uploaded.");
            }

            var file = req.Form.Files[0];
            if (file.Length > 0)
            {
                // Retrieve connection string from environment variables
                string storageConnectionString = Environment.GetEnvironmentVariable("AzureFilesStorage");
                if (string.IsNullOrEmpty(storageConnectionString))
                {
                    log.LogError("AzureFilesStorage connection string is not set.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                try
                {
                    // Parse the connection string
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                    CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

                    // Get a reference to the file share
                    CloudFileShare fileShare = fileClient.GetShareReference("userfiles");
                    if (!await fileShare.ExistsAsync())
                    {
                        await fileShare.CreateAsync();
                        log.LogInformation("Created 'userfiles' file share.");
                    }

                    // Get a reference to the root directory of the share
                    CloudFileDirectory rootDir = fileShare.GetRootDirectoryReference();

                    // Create a file reference with a random GUID name
                    string fileName = Guid.NewGuid().ToString();
                    CloudFile cloudFile = rootDir.GetFileReference(fileName);

                    // Upload the file
                    using (var stream = file.OpenReadStream())
                    {
                        await cloudFile.UploadFromStreamAsync(stream);
                    }

                    string fileUrl = cloudFile.Uri.ToString();
                    log.LogInformation($"File uploaded to Azure Files: {fileUrl}");
                    return new OkObjectResult($"File uploaded successfully. File URL: {fileUrl}");
                }
                catch (StorageException ex)
                {
                    log.LogError($"StorageException: {ex.Message}");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception ex)
                {
                    log.LogError($"Exception: {ex.Message}");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return new BadRequestObjectResult("Uploaded file is empty.");
            }
        }
    }
}
