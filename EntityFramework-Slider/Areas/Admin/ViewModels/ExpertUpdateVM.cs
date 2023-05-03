namespace EntityFramework_Slider.Areas.Admin.ViewModels
{
    public class ExpertUpdateVM
    {
        public string? Header { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Profession { get; set; }
        public IFormFile Photo { get; set; }
    }
}
