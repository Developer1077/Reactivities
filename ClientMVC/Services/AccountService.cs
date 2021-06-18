using ClientMVC.Core;
using ClientMVC.Models;
using ClientMVC.ViewModels;
using Microsoft.AspNetCore.Http;
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
    public class AccountService: BaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IHttpContextAccessor httpContextAccessor,IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //public async Task<Data<User>> GetCurrentUser()
        //{
        //    using var client = new HttpClient();
        //    client.BaseAddress = new Uri(_apiUrl);


        //    var response = await client.GetAsync("account/");
        //    return await HandleResultItem<User>(response);
        //}


        public async Task<Data<User>> Login(LoginViewModel model)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.PostAsJsonAsync("account/login", model);
            return await HandleResult<User>(response);

        }

        public User GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.Request.Cookies["User"];
            if (!String.IsNullOrEmpty(user))
            {

                return JsonSerializer.Deserialize<User>(user);
            }
            return null;
        }
        
        public void SetCurrentUser(User user)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            if (user != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("User", JsonSerializer.Serialize(user), option);               
            }
        }

        public void Logout()
        {
            var user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext.Request.Cookies["User"]);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("User");
            SetCurrentUser(null);

        }

        public async Task<Data<User>> Register(RegisterViewModel model)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.PostAsJsonAsync("account/register", model);
            return await HandleResult<User>(response);
        }
    }
}
