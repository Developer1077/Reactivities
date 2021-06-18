using ClientMVC.Core;
using ClientMVC.Models;
using ClientMVC.Services;
using ClientMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientMVC.Controllers
{
    public class ActivitiesController: BaseController
    {
        private readonly AccountService _accountService;
        private readonly ActivitiesService _activitiesService;
     
        public ActivitiesController(AccountService accountService, ActivitiesService activitiesService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _accountService = accountService;
            _activitiesService = activitiesService;
            
        }

      
        public async Task<IActionResult> Index()
        {
            
            var data = await _activitiesService.GetActivitiesAsync();
            if (data.IsSuccess) {
                var activities = data.Value;
                var activitiesGroupdByDate = activities.GroupBy(activity => activity.Date.ToShortDateString());

                var activitiesDashBoard = new ActivitiesDashBoard
                {
                    Activities = activitiesGroupdByDate,
                    // SelectedActivity = (id == null) ? null : await _activitiesService.GetActivityAsync(id),
                    // EditMode = (edit == "true") ? true : false
                };


                return View(activitiesDashBoard);
            }
            else {
                _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(data.Error));
                return HandleError(data.Error);

            }

        }

        public async Task<IActionResult> Details(string id = null)
        {
            if(id == null) return null;
            var data = await _activitiesService.GetActivityAsync(id);
            if (data.IsSuccess)
            {

                return View(data.Value);
            }
            else
            {
                _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(data.Error));
                return HandleError(data.Error);

            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if(String.IsNullOrEmpty(id)) 
                return View(new ActivityFormViewModel {});

            
            var data = await _activitiesService.GetActivityAsync(id);
            if (data.IsSuccess)
            {

                if (data.Value.HostUsername != _accountService.GetCurrentUser().Username)
                {
                    _toastNotification.AddWarningToastMessage("Unauthorized!!");

                    return RedirectToAction("Details", new{ id = id});
                }
                return View(new ActivityFormViewModel { 
                    SelectedActivity = data.Value                  
                   
                });
            }
            else
            {
                _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(data.Error));
                return HandleError(data.Error);

            }
           
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ActivityFormViewModel model)
        {
            _logger.LogInformation(JsonSerializer.Serialize(model));
            if (ModelState.IsValid) {
                if (model.SelectedActivity.Id != null)
                {
                    //edit
                    var activityData = await _activitiesService.GetActivityAsync(model.SelectedActivity.Id);
                    if (activityData.IsSuccess)
                    {
                        model.SelectedActivity.HostUsername = activityData.Value.HostUsername;
                        //model.SelectedActivity.IsHost = activityData.Value.IsHost;
                        model.SelectedActivity.IsCancelled = activityData.Value.IsCancelled;
                        //model.SelectedActivity.IsGoing = activityData.Value.IsGoing;
                        //model.SelectedActivity.Attendees = activityData.Value.Attendees;
                        //model.SelectedActivity.Host = activityData.Value.Host;

                        _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(model.SelectedActivity));

                        var data = await _activitiesService.UpdateActivityAsync(model.SelectedActivity);
                        if (data.IsSuccess)
                        {
                            return RedirectToAction(nameof(Details), new { id = model.SelectedActivity.Id });
                        }
                        else
                        {
                            return HandleError(data.Error, "edit", "activities", new { id = model.SelectedActivity.Id });
                        }
                    }
                    else
                    {
                        return HandleError(activityData.Error);
                    }                    
                }
                else
                {
                    //create
                   
                    model.SelectedActivity.Id = Guid.NewGuid().ToString();
                    
                    _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(model.SelectedActivity));

                    var data = await _activitiesService.CreateActivityAsync(model.SelectedActivity);
                    if (data.IsSuccess)
                    {
                        return RedirectToAction(nameof(Details), new { id = model.SelectedActivity.Id });
                    }
                    else
                    {
                        return HandleError(data.Error, "edit", "activities");
                    }                    
                }
            }
            _logger.LogWarning("Errew");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var data = await _activitiesService.DeleteActivityAsync(id);
                if (data.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(data.Error));
                    return HandleError(data.Error);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Attend(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var data = await _activitiesService.AttendActivityAsync(id);
                if (data.IsSuccess)
                {
                    
                    return RedirectToAction(nameof(Details), new { id = id});
                }
                else
                {
                    _logger.LogWarning("Errorrrrrr:" + JsonSerializer.Serialize(data.Error));
                    return HandleError(data.Error);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
