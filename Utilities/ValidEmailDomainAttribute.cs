using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Utilities
{
    //Custom validation attribute used for custom validation
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        //Object passed to the method is actually bound email address upcasted to object. It comes from the form on the view. 
        public override bool IsValid(object value)
        {
           
           var strings = value.ToString().Split('@');
           return strings[1].ToUpper() == allowedDomain.ToUpper();
           
        }
    }
}
