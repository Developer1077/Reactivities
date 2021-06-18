using ClientMVC.Core;
using ClientMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Services
{
    public class BaseService
    {
        
        protected ILogger<BaseService> _logger;
        protected IToastNotification _toastNotification;
        protected readonly string _apiUrl;
        public BaseService( IServiceProvider serviceProvider)
        {
            _logger = (ILogger<BaseService>) serviceProvider.GetService(typeof(ILogger<BaseService>));
            _toastNotification = (IToastNotification)serviceProvider.GetService(typeof(IToastNotification));
            _apiUrl= ((IConfiguration)serviceProvider.GetService(typeof(IConfiguration)))["apiUrl"];
        }

        protected async Task<Data<T>> HandleResult<T>(HttpResponseMessage response)
        {
            Data<T> data;
            if (response.IsSuccessStatusCode)
            {
                var readTask = await response.Content.ReadAsAsync<T>();
                data = new Data<T>
                {
                    IsSuccess = true,
                    Value = readTask
                };
            }
            else
            {
                data = HandleError<T>(response);
            }
            return data;

        }

        public Data<T> HandleError<T>(HttpResponseMessage response) {

            var error = new ApiException(StatusCodes.Status400BadRequest, "Bad request, you have made");
            try
            {
                var readTask = response.Content.ReadAsAsync<ApiException>();
                readTask.Wait();
                error = readTask.Result;

                //for notaguid error// normally it returns a validation error response
                if (response.RequestMessage.Method.ToString() == "GET" && error.Errors != null)
                {
                    error = new ApiException(404);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            var data = new Data<T>
            {
                IsSuccess = false,
                Error = error
            };

            return data;
        }
     
    }
}
