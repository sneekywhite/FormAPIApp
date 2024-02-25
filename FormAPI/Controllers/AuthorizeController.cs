using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FormAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly dbContext _context;
        private readonly JwtSetting _jwt;
        public AuthorizeController(dbContext context, IOptions<JwtSetting> options)
        {
            _context = context;
            _jwt = options.Value;


        }

        [HttpPost("GenerateToken")]

        public async Task<IActionResult> GenerateToken(LoginCustomer cred)
        {
            var user = await _context.customers.FirstOrDefaultAsync(item => item.Username == cred.username &&  item.Password == PasswordHash.HashPassword(cred.password));
            if (user != null)
            {
                //generate token
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(_jwt.securitykey);
                var tokendesc = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,user.Username),
                       
                    }),
                    Expires = DateTime.UtcNow.AddSeconds(300),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenhandler.CreateToken(tokendesc);
                var finaltoken = tokenhandler.WriteToken(token);
                return Ok(finaltoken);
            }
            else
            {
                return Unauthorized();
            }
            
        }
    }
}
