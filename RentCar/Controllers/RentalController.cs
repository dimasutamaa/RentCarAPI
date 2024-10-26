using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;
using RentCar.Models.Requests;
using RentCar.Models.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentalController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<RentalController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RentalController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetRentalResult>>> Get(string id)
        {
            var rentals = await _context.trRental
                            .Where(x => x.customer_id == id)
                            .Select(x => new GetRentalResult()
                            {
                                Rental_id = x.Rental_id,
                                rental_date = x.rental_date,
                                return_date = x.return_date,
                                car_name = x.MsCar.name,
                                car_model = x.MsCar.model,
                                price_per_day = x.MsCar.price_per_day,
                                total_price = x.total_price,
                                payment_status = x.payment_status,
                            })
                            .ToListAsync();

            var response = new ApiResponse<IEnumerable<GetRentalResult>>
            {
                StatusCode = StatusCodes.Status200OK,
                RequestMethod = HttpContext.Request.Method,
                Data = rentals
            };
            return Ok(response);
        }

        // POST api/<RentalController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RentCarRequest request, string customer_id, string car_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customer = await _context.msCustomer.FindAsync(customer_id);
                var car = await _context.msCar.FindAsync(car_id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                var nextNumber = await _context.trRental.CountAsync() + 1;
                var rentalId = $"RTD{nextNumber:D3}";

                TimeSpan daysDiff = request.return_date - request.rental_date;

                if (daysDiff.Days < 0)
                {
                    return BadRequest("Return date must be later than the rental date");
                }

                var totalPrice = (daysDiff.Days + 1) * car.price_per_day;

                var newRent = new TrRental()
                {
                    Rental_id = rentalId,
                    rental_date = request.rental_date,
                    return_date = request.return_date,
                    total_price = totalPrice,
                    payment_status = false,
                    customer_id = customer_id,
                    car_id = car_id,
                };

                _context.trRental.Add(newRent);
                await _context.SaveChangesAsync();

                return Ok("Successfully rented a car");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<RentalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RentalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
