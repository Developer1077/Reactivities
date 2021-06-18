using ClientMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.ViewModels
{
    public class ActivityFormViewModel
    {
        public Activity SelectedActivity { get; set; }
        
        public List<SelectListItem> CategoryItems
        {
            get
            {
                return new List<SelectListItem>(){
                    new SelectListItem { Text = "Category", Value = ""},
                    new SelectListItem { Text = "Culture", Value = "culture"},
                    new SelectListItem { Text = "Drinks", Value = "drinks"},
                    new SelectListItem { Text = "Film", Value = "film"},
                    new SelectListItem { Text = "Food", Value = "food"},
                    new SelectListItem { Text = "Music", Value = "music"},
                    new SelectListItem { Text = "Travel", Value = "travel"}
                };
            }

        }

    }
}
