
using ClientMVC.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await _next(context);
            }
            catch (Exception ex) {

                var error = _env.IsDevelopment()
                    ? new ApiException(StatusCodes.Status500InternalServerError, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(StatusCodes.Status500InternalServerError, "Server Error");

                var tempData = _tempDataDictionaryFactory.GetTempData(context);
                tempData["ServerError"] = System.Text.Json.JsonSerializer.Serialize(error);
                tempData.Keep("ServerError");
                context.Response.Redirect("/errors/servererror/");

            }
        
        }

    }
}
