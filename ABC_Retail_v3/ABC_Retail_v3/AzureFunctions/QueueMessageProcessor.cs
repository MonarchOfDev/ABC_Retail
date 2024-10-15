//using System;
//using System.Text;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using Microsoft.WindowsAzure.Storage.Queue;

//namespace ABC_Retail_v3.AzureFunctions
//{
//    public static class QueueMessageProcessor
//    {
//        [FunctionName("QueueMessageProcessor")]
//        public static void Run(
//            [QueueTrigger("registeruser", Connection = "AzureWebJobsStorage")] string encodedMessage,
//            ILogger log)
//        {
//            try
//            {
//                // Decode the base64 message
//                string message = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMessage));
//                log.LogInformation($"C# Queue trigger function processed: {message}");

//                // Implement your processing logic here
//                // For example, log to Table Storage, send notifications, etc.
//            }
//            catch (Exception ex)
//            {
//                log.LogError($"Error processing message: {ex.Message}");
//                // Optionally, handle retries or move the message to a dead-letter queue
//                throw; // Rethrow to allow Azure Functions to handle retries
//            }
//        }
//    }
//}
