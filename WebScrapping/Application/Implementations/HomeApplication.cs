using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebScrapping.Application.Interfaces;
using WebScrapping.Dto.Users;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace WebScrapping.Application.Implementations
{
    public class HomeApplication : IHomeApplication
    {

        public readonly IConfiguration _configuration;
        public HomeApplication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Login(UserDto user)
        {
            return _configuration.GetSection("Users").Get<List<UserDto>>()!.Any(x => x.Username == user.Username && x.Password == user.Password);
        }

        public string CreateToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Secret"] ?? string.Empty));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(90),
                SigningCredentials = cred
            };

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.CreateToken(token);
            return handler.WriteToken(jwt);
        }
    }
}
