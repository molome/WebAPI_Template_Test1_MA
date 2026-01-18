using WebAPI_Template_Test1_MA.Models;

namespace WebAPI_Template_Test1_MA.Services.QueueStorage
{
    public interface IQueueStorageService
    {
        Task SendMessage(EmailMessage emailMessage);
    }
}