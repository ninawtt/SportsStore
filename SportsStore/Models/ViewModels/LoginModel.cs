using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [UIHint("password")] // make the password masked
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/"; // in case the ReturnUrl is empty, we set the default path : root URL of our main page 
    }
}
