using ArcherManager.DAL;
using ArcherManager.Model;
using ArcherManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArcherManager.Web.Controllers
{
    [Route("api/archer")]
    [ApiController]
    public class ArcherApiController : Controller
    {
        private ArcherManagerDbContext _dbContext;

        private ArcherDTO GetArcherDTO(Archer archer)
        {
            ArcherDTO archerDTO = new ArcherDTO()
            {
                Name = archer.Name,
                Surname = archer.Surname,
                ClubId = archer.ClubId,
                Club = archer.Club,
                DateOfBirth = archer.DateOfBirth,
            };
            return archerDTO;
        }

        public ArcherApiController(ArcherManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<Archer[]>> Get()
        {
            ArcherDTO[] archers = _dbContext.Archers.Include(a => a.Club).Select(a => GetArcherDTO(a)).ToArray();
            return Ok(archers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Archer>> Get(int ID)
        {
            Archer archer = await _dbContext.Archers.Include(a => a.Club).FirstOrDefaultAsync(c => c.ID == ID);
            ArcherDTO archerDRO = GetArcherDTO(archer);
            return Ok(archer);
        }

        /*
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/<ArcherApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<ArcherApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
