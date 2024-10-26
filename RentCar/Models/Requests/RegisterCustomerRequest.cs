using System.ComponentModel.DataAnnotations;

namespace RentCar.Models.Requests
{
    public class RegisterCustomerRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string re_password { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [Required]
        public string phone_number { get; set; }
        [Required]
        public string address { get; set; }
    }
}
