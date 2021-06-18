using ClientMVC.Services;
using ClientMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Controllers
{

    public class AccountController : BaseController
    {
       // private readonly ILogger<AccountController> _logger;
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
           
            _accountService = accountService;
        }

        //public IActionResult Logout()
        //{
        //    _accountService.Logout();

        //    return RedirectToAction("index", "home");
        //}

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View(new RegisterViewModel { });
        //}

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel { });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var data = await _accountService.Login(model);
            if (data.IsSuccess)
            {
                _logger.LogInformation(JsonSerializer.Serialize(data.Value));
                _accountService.SetCurrentUser(data.Value);
                return RedirectToAction("index", "activities");
            }
            else
            {
                return HandleError(data.Error, "login", "account");
            }
        }

        public IActionResult Logout()
        {
            _accountService.Logout();

            return RedirectToAction("login", "account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel { });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var data = await _accountService.Register(model);
            if (data.IsSuccess)
            {
                _logger.LogInformation(JsonSerializer.Serialize(data.Value));
                _accountService.SetCurrentUser(data.Value);
                return RedirectToAction("index", "activities");
            }
            else
            {
                return HandleError(data.Error, "register", "account");
            }
        }

    }

}
