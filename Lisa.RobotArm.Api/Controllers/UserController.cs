using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("user")]
    public class UserController
    {
        [HttpGet]
        public async Task<IActionResult> GetUser([FromHeader] string username, [FromHeader] string password)
        {
            object user = await TableStorage.GetUser(username, password);

            if (user == null)
            {
                return new UnprocessableEntityObjectResult("The username or password is not correct or is not registered");
            }

            return new HttpOkObjectResult(user);
        }
    }
}
