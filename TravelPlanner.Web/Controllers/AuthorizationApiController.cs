using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.BusinessLogic.Models;
using TravelPlanner.DomainModel;
using TravelPlanner.Web.Models;
using TravelPlanner.BusinessLogic.Security;
using TravelPlanner.Identity.IdentityManagers;

namespace TravelPlanner.Web.Controllers
{
    public class AuthorizationApiController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthTokenManager _tokenManager;

        public AuthorizationApiController(ApplicationUserManager userManager, IAuthTokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        [Route("api/auth/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return BadRequest(ValidationResultCodes.LoginWrongCredentials);
            }
            bool passwordValid = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!passwordValid)
            {
                return BadRequest(ValidationResultCodes.LoginWrongCredentials);
            }

            var tokens = GetJWTTokens(user);
            return Ok(tokens);
        }

        [Route("api/auth/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegistrationModel model)
        {
            var user = new User()
            {
                Email = model.Email,
                Phone = model.Phone,
                UserName = model.UserName
            };

            if (!String.IsNullOrEmpty(model.CarName) && !String.IsNullOrEmpty(model.CarPetrolUsage))
            {
                user.Cars.Add(new Car() { Name = model.CarName, PetrolUsage = model.CarPetrolUsage });
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault());

            var tokens = GetJWTTokens(user);
            return Ok(tokens);
        }

        private dynamic GetJWTTokens(User user)
        {
            return new
            {
                accessToken = _tokenManager.GetAccessToken(user),
                idToken = _tokenManager.GetIdToken(user)
            };
        }
    }
}
