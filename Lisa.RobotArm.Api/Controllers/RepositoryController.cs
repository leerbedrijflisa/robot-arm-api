using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("repository")]
    public class RepositoryController
    {
        public RepositoryController(TableStorage database)
        {
            _db = database;
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetSingle(string slug)
        {
            dynamic repository = await _db.GetLevel(slug, true);

            if (repository != null)
            {
                return new OkObjectResult(repository.Contents);
            }

            return new NotFoundResult();
        }

        private TableStorage _db;
    }
}