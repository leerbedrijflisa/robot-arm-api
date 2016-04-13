using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("repository")]
    public class RepositoryController
    {
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetSingle(string slug)
        {
            dynamic repository = await TableStorage.GetLevel(slug, true);

            if (repository != null)
            {
                return new HttpOkObjectResult(repository.Contents);
            }

            return new HttpNotFoundResult();
        }
    }
}