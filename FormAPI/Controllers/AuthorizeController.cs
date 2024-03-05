using Azure;
using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Implementation;
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

        private readonly IAuth _auth;

        public AuthorizeController(IAuth auth)
        {
            _auth = auth;

        }

        [HttpPost("GenerateToken")]

        public async Task<IActionResult> GenerateToken(LoginCustomer cred)
        {
            var data = await _auth.Token(cred);
            return Ok(data);
            
 
        }
    }
}
