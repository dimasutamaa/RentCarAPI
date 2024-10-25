using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models.Results
{
    public class GetRentalResult
    {
        public string Rental_id { get; set; }
        public DateTime rental_date { get; set; }
        public DateTime return_date { get; set; }
        public string car_name { get; set; }
        public string car_model { get; set; }
        public decimal price_per_day { get; set; }
        public decimal total_price { get; set; }
        public bool payment_status { get; set; }
        public MsCar MsCar { get; set; }
    }
}
