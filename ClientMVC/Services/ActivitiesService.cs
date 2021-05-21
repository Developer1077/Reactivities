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
    public class ActivitiesService :IActivitiesService
    {
        private readonly string _apiUrl;
        private readonly ILogger<ActivitiesService> _logger;

        public ActivitiesService(ILogger<ActivitiesService> logger, IConfiguration config)
        {
            _apiUrl = config["apiUrl"];
            _logger = logger;
        }

        public async Task<bool> CreateActivityAsync(Activity activity)
        {
            using var client = new HttpClient();
           // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.PostAsJsonAsync($"activities", activity);

            if (response.IsSuccessStatusCode)
            {
                return true;
            } 
            return false;
        }

        public async Task<bool> UpdateActivityAsync(Activity activity)
        {
            using var client = new HttpClient();
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.PutAsJsonAsync($"activities/{activity.Id}", activity);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteActivityAsync(string id)
        {
            using var client = new HttpClient();
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.DeleteAsync($"activities/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<ICollection<Activity>> GetActivitiesAsync()
        {
            using var client = new HttpClient();

            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Accept.Add(contentType);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.BaseAddress = new Uri(_apiUrl);

            ICollection<Activity> activities = new List<Activity>();

            var response = await client.GetAsync("activities/");
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsAsync<ICollection<Activity>>();
                readTask.Wait();

                activities = readTask.Result;
            }
           // _logger.LogInformation(JsonSerializer.Serialize(activities));
            return activities;
        }

        public async Task<Activity> GetActivityAsync(string id)
        {
            using var client = new HttpClient();

            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Accept.Add(contentType);
            client.BaseAddress = new Uri(_apiUrl);

            var activity = new Activity();

            var response = await client.GetAsync($"activities/{id}");
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsAsync<Activity>();
                readTask.Wait();

                activity = readTask.Result;
            }
            // _logger.LogInformation(JsonSerializer.Serialize(activity));
            return activity;
        }
        //public async Task<object> GetMemberAsync(int id)
        //{

        //    using var client = new HttpClient();
        //    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        //    client.DefaultRequestHeaders.Accept.Add(contentType);
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        //    client.BaseAddress = new Uri(_apiUrl);

        //    //Install-Package Microsoft.AspNet.WebApi.Client
        //    using var response = await client.GetAsync($"users/{id}");
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        Member member = new Member();

        //        member = await response.Content.ReadAsAsync<Member>();
        //        return member;
        //    }
        //    //if (response.StatusCode == HttpStatusCode.NotFound)
        //    //{
        //    //    throw new Exception("Cannot get Users");
        //    //}
        //    //else if (response.StatusCode == HttpStatusCode.InternalServerError)
        //    //{
        //    //    throw new Exception();
        //    //}
        //    //else
        //    //{
        //    //    throw new Exception("Cannot get Users");

        //    //}
        //    return response;
        //}

    }
}
