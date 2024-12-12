using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using movies_BLL.DTOs;
using movies_BLL.models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace movies_WebAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public IConfiguration _configuration;
        private readonly DataContext _context;

        public TokenController(IConfiguration config, DataContext context, UserManager<User> userManager)
        {
            _configuration = config;
            _context = context;
            _userManager = userManager;
        }
        
        //LOGIN
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserLoginModel userParam)
        {

            if (userParam != null && userParam.UserName != null && userParam.Password != null)
            {
                var user = await GetUser(userParam.UserName, userParam.Password);

                if (user != null && user.UserName != null)
                {
                    var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName)
                    };

                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims.ToArray(),
                        expires: DateTime.UtcNow.AddMinutes(100),
                        signingCredentials: signIn);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), user });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                Console.WriteLine("badRequest");
                return BadRequest();
            }
        }

        private async Task<User?> GetUser(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            else return null;
        }

        [HttpGet("roles")]
        public List<string> GetRolesFromJwtToken(string token)
        {
            var roles = new List<string>();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                var roleClaims = tokenS.Claims.Where(claim => claim.Type == ClaimTypes.Role).ToList();

                if (roleClaims.Any())
                {
                    roles = roleClaims.Select(claim => claim.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting roles from JWT token: " + ex.Message);
            }

            return roles;
        }

    }
}