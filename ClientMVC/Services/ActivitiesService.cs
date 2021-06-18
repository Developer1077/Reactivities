using ClientMVC.Core;
using ClientMVC.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Services
{
    public class ActivitiesService: BaseService
    {
        private readonly string _token;
        private readonly AccountService _accountService;

        public ActivitiesService(AccountService accountService,IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _accountService = accountService;
            _token = _accountService.GetCurrentUser()?.Token;
        }

        public async Task<Data<ICollection<Activity>>> GetActivitiesAsync()
        {
            using var client = new HttpClient();
            
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.GetAsync("activities/");
            //return await HandleResult<ICollection<Activity>>(response);

            var data = await HandleResult<ICollection<Activity>>(response);
            if (data.IsSuccess)
            {
                
                var activities = new List<Activity>();
                foreach (var activity in data.Value) { 
                    var newactivity = SetActivity(activity);
                    activities.Add(newactivity);
                }
                data.Value = activities;
            }
            return data;
        }


        public async Task<Data<object>> CreateActivityAsync(Activity activity)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            _logger.LogDebug(JsonSerializer.Serialize(activity));
            using var response = await client.PostAsJsonAsync($"activities", activity);

            return await HandleResult<object>(response);

        }

        public async Task<Data<object>> UpdateActivityAsync(Activity activity)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            using var response = await client.PutAsJsonAsync($"activities/{activity.Id}", activity);
            return await HandleResult<object>(response);
        }


        public async Task<Data<object>> AttendActivityAsync(string id)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            using var response = await client.PostAsJsonAsync($"activities/{id}/attend", new { });
            return await HandleResult<object>(response);
        }


        public async Task<Data<object>> DeleteActivityAsync(string id)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.DeleteAsync($"activities/{id}");

            return await HandleResult<object>(response);
        }

        public async Task<Data<Activity>> GetActivityAsync(string id)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Accept.Add(contentType);
            client.BaseAddress = new Uri(_apiUrl);

          
            var response = await client.GetAsync($"activities/{id}");
            var data =  await HandleResult<Activity>(response);
            if (data.IsSuccess)
            {
                var activity = data.Value;
                activity = SetActivity(activity);
                data.Value = activity;
            }
            return data;

        }

        private Activity SetActivity(Activity activity)
        {

            var user = _accountService.GetCurrentUser();
            if (user != null)
            {
                activity.IsGoing = activity.Attendees.Any(a => a.Username == user.Username);
                activity.IsHost = (activity.HostUsername == user.Username);
                activity.Host = activity.Attendees.FirstOrDefault(x => x.Username == activity.HostUsername);
            }
            return activity;

        }

    }
}
