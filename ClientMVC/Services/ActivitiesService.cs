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
        public async Task<ICollection<Activity>> GetActivitiesAsync()
        {
            using var client = new HttpClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
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
