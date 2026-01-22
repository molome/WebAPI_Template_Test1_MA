using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApp.Demo
{
    public class MessageProcessor
    {
        [FunctionName("MessageProcessor")]
        public void Run([QueueTrigger("message-queue", Connection = "AzureStorageAccountConnection")]string myQueueItem, ILogger log)
        {
            //Send an email
            //Validate
            //alert someone

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
