using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI_Template_Test1_MA.Models
{
    public class AttendeeEntity : ITableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Industry { get; set; }


        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }


    public class RequestAttendeeEnitity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Industry { get; set; }
    }

    public class UploadBlobRequest
    {
        public IFormFile File { get; set; }
        public string ImageName { get; set; }
    }
}
