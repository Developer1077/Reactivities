using ClientMVC.Core;
using ClientMVC.Services;
using ClientMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Controllers
{
    public class BaseController : Controller
    {

      
        protected ILogger<BaseController> _logger;
        protected IToastNotification _toastNotification;

        public BaseController(IServiceProvider serviceProvider)
        {
            _logger = (ILogger<BaseController>)serviceProvider.GetService(typeof(ILogger<BaseController>));
            _toastNotification = (IToastNotification)serviceProvider.GetService(typeof(IToastNotification));

        }

        protected IActionResult HandleError(ApiException error, string actionName = null, string controllerName = null, object pageParams = null)
        {
            if (error != null)
            {
                switch (error.StatusCode)
                {
                    case StatusCodes.Status404NotFound:
                        return RedirectToAction("PageNotFound", "Errors");

                    case StatusCodes.Status400BadRequest:
                        if (error.Errors != null)
                        {
                            foreach (var x in error.Errors)
                            {
                                ModelState.AddModelError("", x);
                            }
                            return View(actionName);
                        }
                        else
                        {

                            if (!String.IsNullOrEmpty(actionName))
                            {
                                ModelState.AddModelError("", error.Message);
                                _toastNotification.AddErrorToastMessage(error.Message);

                                return RedirectToAction(actionName, controllerName, pageParams);

                            }
                            else {
                                _toastNotification.AddErrorToastMessage(error.Message);

                            }
                            break;
                        }

                    case StatusCodes.Status500InternalServerError:
                        TempData["ServerError"] = System.Text.Json.JsonSerializer.Serialize(error);
                        return RedirectToAction("ServerError", "Errors");

                    case StatusCodes.Status401Unauthorized:
                        if (!String.IsNullOrEmpty(actionName))
                        {
                            ModelState.AddModelError("", error.Message);
                            return View(actionName);
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage(error.Message);
                            return RedirectToAction("login", "account");
                        }
                        



                    case StatusCodes.Status405MethodNotAllowed:
                        _toastNotification.AddErrorToastMessage("MethodNotAllowed");
                        break;
                   case StatusCodes.Status403Forbidden:
                        _toastNotification.AddWarningToastMessage("You have no privilage to do such action!!");
                        break;
                }
            }
            return RedirectToAction("Index", "home");

        }
    }
}
