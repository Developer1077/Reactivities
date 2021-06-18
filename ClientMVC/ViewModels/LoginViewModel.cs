using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
       // [RegularExpression("(?=0*\\d)(?=.*[a-z](?=.*[A-Z].{4,8}))", ErrorMessage = "Password must be complex")]
        public string Password { get; set; }
        
    }
}
