
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_Slider.Models
{
    public class SliderInfo:BaseEntity
    {
        [Required(ErrorMessage = "Don't be empty")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? SignatureImage { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
