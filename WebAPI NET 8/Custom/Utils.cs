using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI_NET_8.Models;

namespace WebAPI_NET_8.Custom
{

    public class Utils
    {
        private readonly IConfiguration _configuration;
        public Utils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string encryptPassword(string text)
        {
            using (SHA256 sHA256Hash = SHA256.Create())
            {
                byte[] bytes = sHA256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string generateJWT(Usuario model)
        {
            // create user information token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,model.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email,model.Correo!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credntials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //create token details
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credntials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
