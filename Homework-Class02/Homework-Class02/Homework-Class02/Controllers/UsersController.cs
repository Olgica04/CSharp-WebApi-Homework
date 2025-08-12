using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Homework_Class02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet] 
        public ActionResult<List<string>> Get()
        {
            if (StaticDb.Users.Count == 0)
            {
                return NotFound();
            }
            return Ok(StaticDb.Users);
        }

        [HttpGet("{index}")] 
        public ActionResult<string> GetUser(int index)
        {
            try {
                if(index < 0)
                {
                    return BadRequest("You have entered a negative value index!");
                }

                if(index >= StaticDb.Users.Count)
                {
                    return NotFound("The index you have entered is out of reach!");
                }
                return Ok(StaticDb.Users[index]);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administrator!");
            }
        }
    }
}
