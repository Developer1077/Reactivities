using ClientMVC.Services;
using ClientMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<IActionResult> Index(string id = null, string edit = null)
        {
            var activitiesDashBoard = new ActivitiesDashBoard
            {
                Activities = await _activitiesService.GetActivitiesAsync(),
                SelectedActivity = (id == null) ? null : await _activitiesService.GetActivityAsync(id),
                EditMode = (edit == "true") ? true : false
            };
            return View(activitiesDashBoard);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ActivityFormViewModel model)
        {
            if (ModelState.IsValid) { 
                if (model.SelectedActivity.Id!= null) {
                    //edit
                    
                    if (await _activitiesService.UpdateActivityAsync(model.SelectedActivity))
                    {
                        return RedirectToAction(nameof(Index), new { id = model.SelectedActivity.Id });
                    }
                    else
                    {
                        var activitiesDashBoard = new ActivitiesDashBoard
                        {
                            Activities = await _activitiesService.GetActivitiesAsync(),
                            SelectedActivity = model.SelectedActivity,
                            EditMode = true
                        };
                        return View("Index", activitiesDashBoard);
                    }
                }
                else
                {
                    //create
                    model.SelectedActivity.Id = Guid.NewGuid().ToString();

                    if (await _activitiesService.CreateActivityAsync(model.SelectedActivity))
                    {
                        return RedirectToAction(nameof(Index), new { id = model.SelectedActivity.Id });
                    }
                    else 
                    {
                        var activitiesDashBoard = new ActivitiesDashBoard
                        {
                            Activities = await _activitiesService.GetActivitiesAsync(),
                            SelectedActivity = model.SelectedActivity,
                            EditMode = true
                        };

                        return View("Index",activitiesDashBoard);
                    }
                }
            }           
            return View(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                await _activitiesService.DeleteActivityAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
