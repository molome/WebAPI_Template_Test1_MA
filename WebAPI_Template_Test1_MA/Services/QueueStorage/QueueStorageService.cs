using Azure.Storage.Queues;
using Newtonsoft.Json;
using WebAPI_Template_Test1_MA.Models;

namespace WebAPI_Template_Test1_MA.Services.QueueStorage
{
    public class QueueStorageService : IQueueStorageService
    {
        private readonly IConfiguration _configuration;

        public QueueStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessage(EmailMessage emailMessage)
        {
            var queueClient = new QueueClient(_configuration.GetConnectionString("AzureStorageAccountConnection"),
                _configuration["StorageAccount:QueueName"], new QueueClientOptions()
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });

            await queueClient.CreateIfNotExistsAsync();

            var message = JsonConvert.SerializeObject(emailMessage);

            await queueClient.SendMessageAsync(message);

        }


    }
}
