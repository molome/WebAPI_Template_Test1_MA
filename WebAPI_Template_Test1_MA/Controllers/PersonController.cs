using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Template_Test1_MA.DbContexts;
using WebAPI_Template_Test1_MA.Models;

namespace WebAPI_Template_Test1_MA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public PersonController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("Persons")]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await _dbcontext.Persons.ToListAsync();
            return Ok(persons);
        }

        [HttpGet]
        [Route("Persons/{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _dbcontext.Persons.FirstOrDefaultAsync(p => p.PersonId == id);

            if (person != null)
                return Ok(person);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("AddPerson")]
        public async Task<IActionResult> AddPerson(Person person)
        {
            if(ModelState.IsValid)
            {
                await _dbcontext.Persons.AddAsync(person);
                await _dbcontext.SaveChangesAsync();
                return Ok(person);
            }
            else
            {
                return StatusCode(500, "Model State is not valid");
            }
            
        }

    }
}
