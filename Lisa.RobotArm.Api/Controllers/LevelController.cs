using Lisa.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
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
            return new OkObjectResult(levels);
        }

        [HttpGet("{slug}", Name = "slug")]
        public async Task<IActionResult> GetSingle(string slug)
        {
            object level = await _db.GetLevel(slug, false);

            if (level != null)
            {
                return new OkObjectResult(level);
            }

            return new NotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DynamicModel levels)
        {
            if (levels == null)
            {
                return new BadRequestResult();
            }

            dynamic data = levels;
       
            var validatorResults = new LevelValidator().Validate(data);
            if (validatorResults.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validatorResults.Errors); 
            }

            data.slug = Regex.Replace(data.slug.ToString(), @"\s+", "_");
            data.slug = Regex.Replace(data.slug.ToString(), @"[^\w\d]", "");

            var url = Request.Host + "/repository/" + data.slug;
            dynamic level = await _db.PostLevel(data, url);

            if (level == null)
            {
                return new UnprocessableEntityObjectResult(new List<Error>() { new Error { Code = 422, Message = "This slug is already in use.", Values = new { field = "slug", value = data.slug } } });
            }

            string location = Url.RouteUrl("slug", new { slug = data.Slug }, Request.Scheme);
            return new CreatedResult(location, level);
        }

        [HttpPut("{oldSlug}")]
        public async Task<IActionResult> Put([FromBody] DynamicModel levelinput, string oldSlug)
        {
            dynamic data = levelinput;

            var validatorResults = new LevelValidator().Validate(data);
            if (validatorResults.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validatorResults.Errors);
            }

            data.slug = Regex.Replace(data.slug.ToString(), @"\s+", "_");
            data.slug = Regex.Replace(data.slug.ToString(), @"[^\w\d]", "");
            string location = Url.RouteUrl("slug", new { slug = data.Slug }, Request.Scheme);
            var url = Request.Host + "/repository/" + data.slug;

            dynamic levels = await _db.PutLevel(data, url, oldSlug);

            if(levels == null)
            {
                return new UnprocessableEntityObjectResult(new List<Error>() { new Error { Code = 422, Message = "This slug is already in use.", Values = new { field = "slug", value = data.slug } } });
            }
            return new OkObjectResult(levels);
        }

        private TableStorage _db;
    }
}