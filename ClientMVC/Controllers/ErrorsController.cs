using ClientMVC.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Controllers
{
   
    public class ErrorsController: Controller
    {
       
        //[Route("not-found")]
        public IActionResult PageNotFound() {

            return View();
        }

        public IActionResult ServerError()
        {
            if (TempData["ServerError"] != null)
            {
                var model = JsonSerializer.Deserialize<ApiException>(TempData["ServerError"].ToString());
                return View(model);
            }
            return View();

        }

    }
}
