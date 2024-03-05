using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Models;
using FormAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly dbContext _context;
        public CustomerController(ICustomerService customerService, dbContext context)
        {
            _customerService = customerService;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(RegisterCustomer _data)
        {
            var data = await _customerService.Register(_data);
            return Ok(data);

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _customerService.Delete(id);
            return Ok(data);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCust(RegisterCustomer _data, int id)
        {
            var data = await _customerService.Update(_data, id);
            return Ok(data);
        }

        [HttpGet("Getall")]
        [Authorize]
        public async Task<IActionResult> Getall()
        {
            var data = await _customerService.GetAll();
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpGet("GetSingle")]
        //[Route("{id:int}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var data = await _customerService.GetSingle(id);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
