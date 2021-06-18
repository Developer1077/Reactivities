using ClientMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ClientMVC.Controllers.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AccountService _accountService;

        public NavbarViewComponent(ILogger<AccountController> logger, AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _accountService.GetCurrentUser();
         
            return await Task.FromResult(View(user));
        }


    }
}
