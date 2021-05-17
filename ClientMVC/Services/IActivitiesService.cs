using ClientMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientMVC.Services
{
    public interface IActivitiesService
    {
        Task<ICollection<Activity>> GetActivitiesAsync();
    }
}