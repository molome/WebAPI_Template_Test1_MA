using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using WebAPI_Template_Test1_MA.Models;

namespace WebAPI_Template_Test1_MA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EngineerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private Container GetContainerClient()
        {
            var cosmosClient = new CosmosClient(_configuration.GetConnectionString("CosmosDbConnectionString"));
            var database = cosmosClient.GetDatabase(_configuration["CosmosDB:CosmosDbName"]);
            var container = database.GetContainer(_configuration["CosmosDB:CosmosDbContainerName"]);
            return container;
        }

        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> UpsertEngineer(Engineer engineer)
        {
            try
            {
                if(engineer.id == null || engineer.id == Guid.Empty)
                {
                    engineer.id = Guid.NewGuid();
                }

                var container = GetContainerClient();

                var updateRes = await container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));

                Console.WriteLine(updateRes.StatusCode);

                return StatusCode(Convert.ToInt32(updateRes.StatusCode),updateRes.Resource);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteEngineer(string? id)
        {
            try
            {
                var container = GetContainerClient();

                var response = await container.DeleteItemAsync<Engineer>(id, new PartitionKey(id));

                return StatusCode(Convert.ToInt32(response.StatusCode),response.Resource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEngineerDetails()
        {
            List<Engineer> engineers = new List<Engineer>();
            try
            {
                var container = GetContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Engineer> queryResultSetIterator = container.GetItemQueryIterator<Engineer>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Engineer engineer in currentResultSet)
                    {
                        engineers.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(engineers);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEngineerDetailsById(string? id)
        {
            try
            {
                var container = GetContainerClient();
                ItemResponse<Engineer> response = await container.ReadItemAsync<Engineer>(id, new PartitionKey(id));
                return StatusCode(Convert.ToInt32(response.StatusCode), response.Resource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
