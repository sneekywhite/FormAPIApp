using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Dto.Response;
using FormAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FormAPI.Implementation
{
    public class Auth : IAuth

    {
        private readonly dbContext _context;
        private readonly JwtSetting _jwt;
        public Auth(dbContext context, IOptions<JwtSetting> options)
        {
            _context = context;
            _jwt = options.Value;
        }
        public async Task<Response> Token(LoginCustomer cred)
        {
            Response _response = new Response();
            try
            {
                var user = await _context.customers.FirstOrDefaultAsync(item => item.Email == cred.email && item.Password == PasswordHash.HashPassword(cred.password));
                if (user != null)
                {
                    //generate token
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.UTF8.GetBytes(_jwt.securitykey);
                    var tokendesc = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Email,user.Email),

                        }),
                        Expires = DateTime.UtcNow.AddSeconds(300),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenhandler.CreateToken(tokendesc);
                    var finaltoken = tokenhandler.WriteToken(token);

                    _response.Result = finaltoken;
                    _response.Message = "Succesfully Authorize";
                    _response.IsSuccess = true;

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Wrong email or password";
                    _response.Result = "";
                }
                
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            

            return _response;
        }
    }

}
