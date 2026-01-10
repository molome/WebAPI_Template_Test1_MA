using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Template_Test1_MA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        List<string> employeeNameList = new List<string>()
        {
            "Mohsin Ahmed",
            "Mohammed Mohalil",
            "Zakir Hussain"
        };

        Dictionary<int, string> empList = new Dictionary<int, string>()
        {
            {1, "Mohsin Ahmed" },
            {2, "Mohammed Mohalil" },
            {3, "Zakir Hussain" }
        };

        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            return Ok(employeeNameList);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            
            return Ok(empList.GetValueOrDefault(id));
        }

    }
}
