using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;


namespace EntityFramework_Slider.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }

        //[Required(ErrorMessage = "Don't be be empty")]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
