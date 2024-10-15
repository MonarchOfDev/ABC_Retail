using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Linq;

namespace ABCRetail_Functions_v9
{
    public static class WriteToBlobStorage
    {
        [FunctionName("WriteToBlobStorage")]
        public static async Task<IActionResult> Run(
                    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "uploadBlob")] HttpRequest req,
                    [Blob("useruploads/{rand-guid}", FileAccess.Write, Connection = "AzureBlobStorage")] CloudBlockBlob blob,
                    ILogger log)
        {
            log.LogInformation("WriteToBlobStorage function processed a request.");

            if (!req.Form.Files.Any())
            {
                return new BadRequestObjectResult("No file uploaded.");
            }

            var file = req.Form.Files[0];
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadFromStreamAsync(stream);
                }

                string blobUrl = blob.Uri.ToString();
                log.LogInformation($"File uploaded to Blob Storage: {blobUrl}");
                return new OkObjectResult($"File uploaded successfully. Blob URL: {blobUrl}");
            }
            else
            {
                return new BadRequestObjectResult("Uploaded file is empty.");
            }
        }
    }
}
