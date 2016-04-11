using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("user")]
    public class UserController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            object user = await TableStorage.GetUser(username);

            if (user == null)
            {
                return new UnprocessableEntityObjectResult("The username is not correct or is not registered");
            }

            return new HttpOkObjectResult(user);
        }
    }
}
