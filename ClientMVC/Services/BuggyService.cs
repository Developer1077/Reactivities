using ClientMVC.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.Services
{
    public class BuggyService: BaseService
    {
        private readonly AccountService _accountService;
        private readonly string _token;

        public BuggyService(AccountService accountService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _accountService = accountService;
            _token = _accountService.GetCurrentUser()?.Token;
        }


        public async Task<Data<string>> GetNotFound()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            using var response = await client.GetAsync("buggy/not-found");
            return await HandleResult <string>(response);
        }

        public async Task<Data<string>> GetBadRequest()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            using var response = await client.GetAsync("buggy/bad-request");
            return await HandleResult<string>(response);
        }

        public async Task<Data<string>> GetUnauthorised()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);

            var response = await client.GetAsync("buggy/unauthorised");
            return await HandleResult <string>(response);
        }
        public async Task<Data<string>> GetServerError()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            using var response = await client.GetAsync("buggy/server-error");

            return await HandleResult <string>(response);
        }
        public async Task<Data<string>> GetValidationError()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            using var response = await client.PostAsJsonAsync("activities", new {});
            return await HandleResult<string>(response);

        }      
        public async Task<Data<string>> GetNotAGuid()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.GetAsync("activities/notaguid");

            return await HandleResult<string>(response);
        }
    }
}
