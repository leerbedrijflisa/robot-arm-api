using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("user")]
    public class UserController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
        }
    }
}
