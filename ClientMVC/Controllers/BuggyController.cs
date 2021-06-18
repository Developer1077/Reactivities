using ClientMVC.Services;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.Controllers
{
    public class BuggyController:BaseController
    {
       private readonly BuggyService _buggyService;

       
        public BuggyController(BuggyService buggyService,  IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _buggyService = buggyService;
        }

        public IActionResult Index() {
            return View();

        }
        public async Task<IActionResult> GetNotFound()
        {
            var data = await _buggyService.GetNotFound();
            if (data.IsSuccess){
                return View(nameof(Index));
            }
            else{
                return HandleError(data.Error);
            }           
        }

        public async Task<IActionResult> GetBadRequest()
        {

            var data = await _buggyService.GetBadRequest();
            if (data.IsSuccess)
            {
                return View(nameof(Index));
            }
            else
            {
                return HandleError(data.Error,"index");
            }
        }

        public async Task<IActionResult> GetValidationError()
        {
            var data = await _buggyService.GetValidationError();
            if (data.IsSuccess)
            {
                return View(nameof(Index));
            }
            else
            {
                return HandleError(data.Error,"index","buggy");
            }

        }

        public async Task<IActionResult> GetUnauthorised()
        {
            var data = await _buggyService.GetUnauthorised();
            if (data.IsSuccess)
            {
                return View(nameof(Index));
            }
            else
            {
                return HandleError(data.Error);
            }
        }


        public async Task<IActionResult> GetServerError()
        {
            var data = await _buggyService.GetServerError();
            if (data.IsSuccess)
            {
                return View(nameof(Index));
            }
            else
            {
                return HandleError(data.Error);
            }
        }


        public async Task<IActionResult> GetBadGuid()
        {
            var data = await _buggyService.GetNotAGuid();
            if (data.IsSuccess)
            {
                return View(nameof(Index));
            }
            else
            {
                return HandleError(data.Error, "index", "buggy");
            }
        }
    }
}
