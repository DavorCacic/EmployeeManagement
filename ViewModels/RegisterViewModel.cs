using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterViewModel
    {   
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+$", ErrorMessage = "Invalid email format!")]
        [EmailAddress] //Predefined validation attribute for e-mail address. 
        [ValidEmailDomain(allowedDomain: "osvezenje.com", ErrorMessage = "Domain must be \"osvezenje.com.\"")] //custom validation logic attribute - to check if provided email address is in specified domain. Note that ASP is "smart", so "Attribute" on the name of the class is not needed.  
        [Remote(action: "IsEmailInUse", controller: "Account")] // Ajax call for remote validation. Server side method IsEmailInUse is defined in Account controller. Don't forget that client side libraries are needed (in Layout view).
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //Attribute for vaidation, but masking characters on the view as well. 
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")] //How it will be shown on the view.
        [Compare("Password", ErrorMessage ="Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
}
