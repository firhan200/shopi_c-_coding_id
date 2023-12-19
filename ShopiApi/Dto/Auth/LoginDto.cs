using System.ComponentModel;

namespace ShopiApi.Dto.Auth
{
    public class LoginDto
    {
        [DefaultValue("test@email.com")]
        public string Email { get; set; } = string.Empty;
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}
