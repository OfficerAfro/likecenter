using System;
using System.ComponentModel.DataAnnotations;

namespace likecenter.Models
{
    public class LUser
    {
        [Required(ErrorMessage="Email is required")]
        [EmailAddress]
        public string LEmail {get;set;}

        [Required(ErrorMessage="Password is required")]
        [DataType(DataType.Password)]
        public string LPassword {get;set;}
    }
}