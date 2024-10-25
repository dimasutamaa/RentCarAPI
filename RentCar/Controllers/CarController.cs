using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<CarController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCarResult>>> Get()
        {
            var cars = await _context.msCar
                        .Include(x => x.car_images)
                        .Select(x => new GetCarResult()
                        {
                            Car_id = x.Car_id,
                            name = x.name,
                            model = x.model,
                            year = x.year,
                            license_plate = x.license_plate,
                            number_of_car_seats = x.number_of_car_seats,
                            transmission = x.transmission,
                            price_per_day = x.price_per_day,
                            status = x.status,
                            car_images = x.car_images,
                        })
                        .ToListAsync();

            var response = new ApiResponse<IEnumerable<GetCarResult>>
            {
                StatusCode = StatusCodes.Status200OK,
                RequestMethod = HttpContext.Request.Method,
                Data = cars
            };
            return Ok(response);
        }

        // GET api/<CarController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CarController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CarController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CarController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
