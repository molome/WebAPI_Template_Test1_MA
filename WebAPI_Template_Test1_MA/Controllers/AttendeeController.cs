using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI_Template_Test1_MA.Models;
using WebAPI_Template_Test1_MA.Services.BlobStorage;
using WebAPI_Template_Test1_MA.Services.QueueStorage;
using WebAPI_Template_Test1_MA.Services.TableStorage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_Template_Test1_MA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IQueueStorageService _queueStorageService;

        public AttendeeController(ITableStorageService tableStorageService, IBlobStorageService blobStorageService, IQueueStorageService queueStorageService)
        {
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
            _queueStorageService = queueStorageService;
        }
        // GET: api/<AttendeeController>
        [HttpGet]
        public async Task<IActionResult> GetAttendees()
        {
            try
            {
                var attendees = await _tableStorageService.GetAttendees();
                return Ok(attendees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<AttendeeController>/5
        [HttpGet("{industry}/{id}")]
        public async Task<IActionResult> GetAttendee(string industry, string id)
        {
            try
            {
                var attendee = await _tableStorageService.GetAttendee(industry, id);
                return Ok(attendee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<AttendeeController>
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddAttendee(RequestAttendeeEnitity requestAttendeeEntity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AttendeeEntity attendeeEntity = new AttendeeEntity();

                    attendeeEntity.PartitionKey = requestAttendeeEntity.Industry;
                    attendeeEntity.RowKey = Guid.NewGuid().ToString();

                    attendeeEntity.FirstName = requestAttendeeEntity.FirstName;
                    attendeeEntity.LastName = requestAttendeeEntity.LastName;
                    attendeeEntity.EmailAddress = requestAttendeeEntity.EmailAddress;
                    attendeeEntity.Industry = requestAttendeeEntity.Industry;

                    await _tableStorageService.UpsertAttendee(attendeeEntity);

                    return StatusCode(201);
                }
                else
                {
                    var errors = ModelState
                                .Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

                    return BadRequest(errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<AttendeeController>/5
        [HttpPut]
        [Route("Update/{industry}/{id}")]
        public async Task<IActionResult> UpdateAttendee(string industry, string id, [FromBody] RequestAttendeeEnitity requestAttendeeEntity)
        {
            try
            {
                var attendee = await _tableStorageService.GetAttendee(industry, id);

                if (attendee != null)
                {
                    if (ModelState.IsValid)
                    {
                        AttendeeEntity attendeeEntity = new AttendeeEntity();

                        attendeeEntity.PartitionKey = requestAttendeeEntity.Industry;
                        attendeeEntity.RowKey = attendee.RowKey;

                        attendeeEntity.FirstName = requestAttendeeEntity.FirstName;
                        attendeeEntity.LastName = requestAttendeeEntity.LastName;
                        attendeeEntity.EmailAddress = requestAttendeeEntity.EmailAddress;
                        attendeeEntity.Industry = requestAttendeeEntity.Industry;

                        await _tableStorageService.UpsertAttendee(attendeeEntity);

                        return Ok();
                    }
                    else
                    {
                        var errors = ModelState
                                    .Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();

                        return BadRequest(errors);
                    }
                }
                else
                {
                    return NotFound("Attendee Not found with given Id's");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<AttendeeController>/5
        [HttpDelete("Delete/{industry}/{id}")]
        public async Task<IActionResult> DeleteAttendee(string industry, string id)
        {
            try
            {
                await _tableStorageService.DeleteAttendee(industry, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("GetBlobUrl/{imageName}")]
        public async Task<IActionResult> GetBlobUrl(string imageName)
        {
            try
            {
                var imageUrl = await _blobStorageService.GetBlobUrl(imageName);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("UploadBlob")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadBlob(UploadBlobRequest uploadBlobRequest)
        {
            try
            {
                var imgName = await _blobStorageService.UploadBlob(uploadBlobRequest.File,uploadBlobRequest.ImageName);
                return Ok(imgName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteBlob/{imageName}")]
        public async Task<IActionResult> DeleteBlob(string imageName)
        {
            try
            {
                await _blobStorageService.RemoveBlob(imageName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("SendQueueMessage")]
        public async Task<IActionResult> SendQueueMessage(RequestAttendeeEnitity requestAttendeeEnitity)
        {
            try
            {
                var email = new EmailMessage()
                {
                    EmailAddress = requestAttendeeEnitity.EmailAddress,
                    TimeStamp = DateTime.UtcNow,
                    Message = $"Hello {requestAttendeeEnitity.FirstName} {requestAttendeeEnitity.LastName}, " +
                    $"\n\r Thank you for registering for this event. " +
                    $"\n\r Your record has been saved for your future reference. "
                };

                await _queueStorageService.SendMessage(email);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
