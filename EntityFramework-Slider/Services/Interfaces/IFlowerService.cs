using EntityFramework_Slider.Models;

namespace EntityFramework_Slider.Services.Interfaces
{
    public interface IFlowerService
    {
        Task<IEnumerable<Expert>> GetAll();
        Task<IEnumerable<ExpertHeader>> GetInfos();
    }
}
