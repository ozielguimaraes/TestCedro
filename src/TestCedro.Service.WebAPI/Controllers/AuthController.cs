using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestCedro.Domain.Interfaces.Services;
using TestCedro.Service.WebAPI.Auth;
using TestCedro.Service.WebAPI.Helpers;
using TestCedro.Service.WebAPI.Models;
using TestCedro.Service.WebAPI.ViewModels;

namespace TestCedro.Service.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _userService;

        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IUserService userService)
        {
            _jwtFactory = jwtFactory;
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }

        // POST v1/auth/signin
        [HttpPost("signin")]
        public async Task<IActionResult> Post([FromBody]LoginVm credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Usuário ou senha incorreta.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);
            
            var userToVerify = await _userService.GetByUserEmailAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);
            
            if (await _userService.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.UserId));
            }
            
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}