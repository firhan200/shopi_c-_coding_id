using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopiApi.Dto.Auth;
using ShopiApi.Dto.Product;
using ShopiApi.Helpers;
using ShopiApi.Models;
using ShopiApi.Repositories;
using ShopiApi.Services;

namespace ShopiApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("/Login")]
        public ActionResult Login([FromBody] LoginDto data)
        {
            string hashedPassword = PasswordHelper.HashPassword(data.Password);

            User? user = _userRepository.GetByEmailAndPassword(data.Email, hashedPassword);

            if (user == null)
            {
                return NotFound();
            }

            //create token
            string token = JWTHelper.Generate(user.Id, user.Role);

            return Ok(token);
        }
    }
}
