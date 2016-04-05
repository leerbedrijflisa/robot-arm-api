using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api
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
        public async Task<IActionResult> GetSingle(string slug)
        {
            object level = await TableStorage.GetLevel(slug, false);

            if (level != null)
            {
                return new HttpOkObjectResult(level);
            }

            return new HttpNotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DynamicModel levels)
        {
            if (levels == null)
            {
                return new BadRequestResult();
            }

            var validatorResults = new LevelValidator().Validate(levels);
            if (validatorResults.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validatorResults.Errors); 
            }
            dynamic level = await TableStorage.PostLevel(levels);

            if (level)
            {
                return new BadRequestResult();
            }

            string location = Url.RouteUrl("slug", new { slug = level.Slug }, Request.Scheme);

            return new CreatedResult(location, level);
        }
    }
}