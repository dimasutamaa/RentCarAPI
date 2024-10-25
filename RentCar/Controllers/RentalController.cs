using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
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
        public void Post([FromBody] string value)
        {
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
