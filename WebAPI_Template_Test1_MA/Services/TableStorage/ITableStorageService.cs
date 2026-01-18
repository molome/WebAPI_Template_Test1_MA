using WebAPI_Template_Test1_MA.Models;

namespace WebAPI_Template_Test1_MA.Services.TableStorage
{
    public interface ITableStorageService
    {
        Task<AttendeeEntity> GetAttendee(string industry, string id);
        Task<List<AttendeeEntity>> GetAttendees();
        Task UpsertAttendee(AttendeeEntity attendeeEntity);
        Task DeleteAttendee(string industry, string id);

    }
}