using ClientMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientMVC.Services
{
    public interface IActivitiesService
    {
        Task<ICollection<Activity>> GetActivitiesAsync();
        Task<Activity> GetActivityAsync(string id);
        Task<bool> CreateActivityAsync(Activity activity);
        Task<bool> UpdateActivityAsync(Activity activity);

        Task<bool> DeleteActivityAsync(string id);

    }
}
