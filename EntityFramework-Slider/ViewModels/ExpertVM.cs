using EntityFramework_Slider.Models;

namespace EntityFramework_Slider.ViewModels
{
    public class ExpertVM
    {
        public IEnumerable<Expert> Experts { get; set; }
        public IEnumerable<ExpertHeader> ExpertHeaders { get; set; }
    }
}
