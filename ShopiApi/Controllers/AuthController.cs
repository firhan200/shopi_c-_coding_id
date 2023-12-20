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

            return Ok(new { Token = token, Role = user.Role });
        }

        [HttpPost("/Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto data)
        {
            string hashedPassword = PasswordHelper.HashPassword(data.Password);
            string verificationToken = Guid.NewGuid().ToString();

            // check if email already taken
            User? user = _userRepository.GetByEmail(data.Email);
            if (user != null)
            {
                return BadRequest("Email Already Taken");
            }

            string errorMessage = _userRepository.Register(data.Email, hashedPassword, verificationToken);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Problem("Error when registering user");
            }

            //send email
            string htmlEmail = $@"
Hello <b>{data.Email}</b>, please click link below to verify<br/>
<a href='http://localhost:5173/verify/{verificationToken}'>Verify My Account</a>
";
            await MailHelper.Send("Dear User", data.Email, "Email Verification", htmlEmail);

            return Ok();
        }

        [HttpPost("/Verify")]
        public ActionResult Verify([FromBody] VerifyDto data)
        {
            User? user = _userRepository.GetByToken(data.Token);
            if (user == null)
            {
                return NotFound();
            }

            //check if token is expired or not
            if(user.VerificationExpiredDate <= DateTime.Now) {
                return Problem("Token are expired");
            }

            // activate user
            string errorMessage = _userRepository.Activate(user.Id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Problem(errorMessage);
            }

            return Ok();
        }
    }
}
