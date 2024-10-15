using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ABC_Retail_v3.Models;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ABCRetail_Functions_v9
{
    public class ProcessQueueMessage
    {
        [FunctionName("ProcessQueueMessage")]
        public static async Task Run(
            [QueueTrigger("transactionqueue", Connection = "AzureQueueStorage")] string queueMessage,
            ILogger log)
        {
            log.LogInformation($"ProcessQueueMessage function received: {queueMessage}");

            try
            {
                // Assuming the message is JSON serialized CraftUsers object
                var user = JsonConvert.DeserializeObject<CraftUsers>(queueMessage);

                if (user == null)
                {
                    log.LogWarning("Deserialized user is null.");
                    return;
                }

                // Implement your transaction processing logic here
                // For example, updating user status, sending notifications, etc.

                log.LogInformation($"Processed transaction for user: {user.Email}");

                // Optionally, you can interact with other Azure services here
            }
            catch (JsonException ex)
            {
                log.LogError($"JSON deserialization error: {ex.Message}");
                // Optionally, move the message to a dead-letter queue
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing queue message: {ex.Message}");
                // Optionally, implement retry logic
                throw; // Rethrow to allow Azure Functions to handle retries
            }
        }
    }
}
