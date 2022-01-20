using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+$", ErrorMessage = "Invalid email format!")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        [Required]
        public Department? Department { get; set; }
        public string PhotoPath { get; set; }

        //To validate model in the form:
        //1. Apply Validation Attributes on the properties in model class
        //2. Check the model state in the controler (ModelState.IsValid)
        //3. Use asp-validation-for and asp-validation-summary tag helpers on the view

        //Department property is nullable because validation of enums works better this way. Without, validation message will just say - Valsue "" is not valid. 
        //With this, it will say name of the property. And, of course, Option value elelemnt has to be created on the view ("Please Select"). 

    }
}
