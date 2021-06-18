using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
    //    [RegularExpression("(?=0*\\d)(?=.*[a-z](?=.*[A-Z].{4,8}))", ErrorMessage = "Password must be complex")]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }

    }
}
