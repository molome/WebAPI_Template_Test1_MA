using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Template_Test1_MA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        List<string> employeeNameList = new List<string>()
        {
            "Mohsin Ahmed",
            "Mohammed Mohalil",
            "Zakir Hussain",
            "Nizammuddin"
        };

        Dictionary<int, string> empList = new Dictionary<int, string>()
        {
            {1, "Mohsin Ahmed" },
            {2, "Mohammed Mohalil" },
            {3, "Zakir Hussain" },
            {4, "Nizammuddin" }
        };

        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            return Ok(employeeNameList);
        }

        [HttpGet]
        [Route("Greetings")]
        public IActionResult GetGreetings()
        {
            return Ok(_configuration["Greeting"]);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            if(id == 4)
            {
                throw new Exception("Error Occurred");
            }
            
            return Ok(empList.GetValueOrDefault(id));
        }

    }
}
