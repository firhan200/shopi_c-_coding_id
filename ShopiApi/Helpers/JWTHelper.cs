using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopiApi.Helpers
{
    public static class JWTHelper
    {
        public static string KEY = "a01cca948b5c8cb9ab691acbc8a6e561f129ee3fa01cca948b5c8cb9ab691acbc8a6e561f129ee3fa01cca948b5c8cb9ab691acbc8a6e561f129ee3f";
        public static string Generate(int userId, string role)
        {
            //init claims payload
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
            };

            //set jwt config
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)), SecurityAlgorithms.HmacSha512)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
