using Lisa.Common.WebApi;
using Lisa.RobotArm.Api.Database;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("levels")]
    public class LevelController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<object> levels = await TableStorage.GetLevels();
            return new HttpOkObjectResult(levels);
        }

        [HttpGet("{slug}", Name = "slug")]
        public async Task<IActionResult> GetSingle(String Slug)
        {
            object level = await TableStorage.GetLevel(Slug);

            if (level != null)
            {
                return new HttpOkObjectResult(level);
            }

            return new HttpNotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DynamicModel levels)
        {
            dynamic level = await TableStorage.PostLevel(levels);

            string location = Url.RouteUrl("slug", new { slug = level.Slug }, Request.Scheme);

            return new CreatedResult(location, level);
        }
    }
}
