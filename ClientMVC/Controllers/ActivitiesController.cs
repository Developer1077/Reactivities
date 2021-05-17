using ClientMVC.Services;
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
    public class ActivitiesController: Controller
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly IActivitiesService _activitiesService;

        public ActivitiesController(ILogger<ActivitiesController> logger,IActivitiesService activitiesService)
        {
            _logger = logger;
            _activitiesService = activitiesService;
        }
        public async Task<IActionResult> Index() {

            var activities = await _activitiesService.GetActivitiesAsync();

             //_logger.LogInformation(JsonSerializer.Serialize(activities));
            return View(activities);
        }
    }
}
