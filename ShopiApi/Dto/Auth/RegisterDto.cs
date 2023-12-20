using System.ComponentModel;

namespace ShopiApi.Dto.Auth
{
    public class RegisterDto
    {
        [DefaultValue("firhan.faisal1995@gmail.com")]
        public string Email { get; set; } = string.Empty;
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}
