using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Lisa.RobotArm.Api.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        public UserController(TableStorage database)
        {
            _db = database;
        }
        [HttpGet("{firstName}")]
        public async Task<IActionResult> GetSingle(string firstName)
        {
            dynamic user = await _db.GetUser(firstName);

            if (user != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.firstName, ClaimValueTypes.String));
                var identity = new ClaimsIdentity(new GenericIdentity(user.firstName, "TokenAuth"), claims);

                // RSAParameters keyParams = RSAKeyUtils.GetRandomKey();
                // key = new RsaSecurityKey(keyParams);

                // var securityToken = handler.CreateToken(
                //     signingCredentials: new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature),
                //     subject: identity
                //);
                return new OkObjectResult(user.firstName);
            }

            return new NotFoundResult();
        }

        private TableStorage _db;
    }
}
