using ClientMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.ViewModels
{
    public class ActivitiesDashBoard
    {
        public IEnumerable<IGrouping<string,Activity>> Activities { get; set; }
       // public Activity SelectedActivity { get; set; }        
        //public bool EditMode { get; set; } = false;
       

    }
}
