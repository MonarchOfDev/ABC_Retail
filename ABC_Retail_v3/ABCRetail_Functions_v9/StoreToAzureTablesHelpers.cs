using ABC_Retail_v3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;

internal static class StoreToAzureTablesHelpers
{
    [FunctionName("StoreToAzureTables")]
    public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "post", Route = "storeUser")] HttpRequest req,
                //[Table(name: "CraftUsers", Schema = "")]
                 IAsyncCollector<CraftUsers> tableBinding,
                ILogger log)
    {
        log.LogInformation("StoreToAzureTables function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        CraftUsers user;

        try
        {
            user = JsonConvert.DeserializeObject<CraftUsers>(requestBody);
        }
        catch (JsonException ex)
        {
            log.LogError($"JSON deserialization error: {ex.Message}");
            return new BadRequestObjectResult("Invalid JSON format.");
        }

        if (user == null)
        {
            return new BadRequestObjectResult("User data is required.");
        }

        // Validate user data
        try
        {
            user.ValidateUser();
        }
        catch (ArgumentException ex)
        {
            log.LogError($"Validation error: {ex.Message}");
            return new BadRequestObjectResult(ex.Message);
        }




        // Ensure PartitionKey and RowKey are set
        if (string.IsNullOrEmpty(user.PartitionKey))
        {
            user.PartitionKey = "User"; // Or another strategy, e.g., based on role
        }

        if (string.IsNullOrEmpty(user.RowKey))
        {
            user.RowKey = Guid.NewGuid().ToString();
        }

        // Add to Table Storage
        try
        {
            await tableBinding.AddAsync(user);
            log.LogInformation($"User {user.Email} stored successfully in Azure Table Storage.");
            return new OkObjectResult($"User {user.Email} registered successfully.");
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
}