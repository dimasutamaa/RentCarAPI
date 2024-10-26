using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models.Requests
{
    public class RentCarRequest
    {
        [Required]
        public string Rental_id { get; set; }
        [Required]
        public DateTime rental_date { get; set; }
        [Required]
        public DateTime return_date { get; set; }
        [Required]
        public decimal total_price { get; set; }
        [Required]
        public bool payment_status { get; set; }
        [Required]
        public string customer_id { get; set; }
        [Required]
        public string car_id { get; set; }
    }
}
