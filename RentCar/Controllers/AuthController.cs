using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;
using RentCar.Models.Requests;
using RentCar.Models.Results;
using RentCar.Services;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;

        public AuthController(AppDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // POST api/<AuthController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var emailExists = await _context.msCustomer.AnyAsync(x => x.email == request.email);

                if (emailExists)
                {
                    return Conflict("Email already exists");
                }

                if (request.re_password != request.password)
                {
                    return BadRequest("Passwords do not match");
                }

                var nextNumber = await _context.msCustomer.CountAsync() + 1;
                var customerId = $"CUS{nextNumber:D3}";

                var hashedPassword = _passwordService.HashPassword(request.password);

                var customer = new MsCustomer
                {
                    Customer_id = customerId,
                    name = request.name,
                    email = request.email,
                    password = hashedPassword,
                    phone_number = request.phone_number,
                    address = request.address
                };

                _context.msCustomer.Add(customer);
                await _context.SaveChangesAsync();

                var response = new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status201Created,
                    RequestMethod = HttpContext.Request.Method,
                    Data = "Customer registered successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customer = await _context.msCustomer.SingleOrDefaultAsync(x => x.email == request.email);

                if (customer == null)
                {
                    return NotFound("Invalid email or password");
                }

                var isPasswordValid = _passwordService.VerifyPassword(customer, request.password);

                if (!isPasswordValid)
                {
                    return BadRequest("Invalid email or password");
                }

                var response = new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    RequestMethod = HttpContext.Request.Method,
                    Data = "Customer successfully logged in"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
