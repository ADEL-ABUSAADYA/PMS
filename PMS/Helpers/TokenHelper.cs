using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PMS.Helpers
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Guid userID)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
             
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("ID", userID.ToString()),
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        
        public string Generate2FALoginToken(Guid userId)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["OTPSettings:SecretKey"]);
            var issuer = _configuration["OTPSettings:Issuer"];
            var audience = _configuration["OTPSettings:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID", userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        
        public int? Validate2FALoginToken(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_configuration["OTPSettings:SecretKey"]);
                var issuer = _configuration["OTPSettings:Issuer"];
                var audience = _configuration["OTPSettings:Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true, // Ensure the token is not expired
                    ClockSkew = TimeSpan.Zero // No extra time tolerance
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Ensure the token is a JWT token
                if (validatedToken is not JwtSecurityToken jwtToken)
                    return null;

                // Extract the user ID claim
                var userIdClaim = principal.FindFirst("ID")?.Value;
                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch
            {
                return null; // Invalid token
            }
        }
    }
}
