using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models
{
    public class MsCarImages
    {
        [Key]
        public string Image_car_id { get; set; }
        public string image_link { get; set; }
        [ForeignKey("MsCar")]
        public string Car_id { get; set; }
        public MsCar MsCar { get; set; }
    }
}
