using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("user")]
    public class UserController
    {
        public UserController(TableStorage database)
        {
            _db = database;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromHeader] string username, [FromHeader] string password)
        {
            object user = await _db.GetUser(username, password);

            if (user == null)
            {
                return new UnprocessableEntityObjectResult("The username or password is not correct or is not registered");
            }

            return new HttpOkObjectResult(user);
        }

        private TableStorage _db;
    }
}
