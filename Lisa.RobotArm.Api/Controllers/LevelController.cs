using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api
{
    [Route("levels")]
    public class LevelController : Controller
    {
        public LevelController(TableStorage database)
        {
            _db = database;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<object> levels = await _db.GetLevels();
            return new HttpOkObjectResult(levels);
        }

        [HttpGet("{slug}", Name = "slug")]
        public async Task<IActionResult> GetSingle(string slug)
        {
            object level = await _db.GetLevel(slug, false);

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

            dynamic data = levels;
            data.slug = Regex.Replace(data.slug.ToString(), @"[^\w\d]", "");
            string location = Url.RouteUrl("slug", new { slug = data.Slug }, Request.Scheme);

            var validatorResults = new LevelValidator().Validate(data);
            if (validatorResults.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validatorResults.Errors); 
            }

            dynamic level = await _db.PostLevel(data, location);

            if (level == null)
            {
                return new UnprocessableEntityObjectResult(new List<Error>() { new Error { Code = 422, Message = "This slug is already in use.", Values = new { field = "slug", value = data.slug } } });
            }

            return new CreatedResult(location, level);
        }

        private TableStorage _db;
    }
}