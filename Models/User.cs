using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace likecenter.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="Name is required")]
        [MinLength(2,ErrorMessage="longer please")]
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage = "Letters and spaces!")]
        public string Name {get;set;}
        [Required(ErrorMessage="Alias is required")]
        [MinLength(2,ErrorMessage="longer please")]
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage = "Letters and spaces!")]
        public string Alias {get;set;}

        [Required(ErrorMessage="Email is required")]
        [EmailAddress]
        public string Email {get;set;}

        [Required(ErrorMessage="Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", 
         ErrorMessage = "Password must meet requirements of one special character one uppercase letter one lower case number and one number while being 8 characters or longer")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        public int LikeCounter {get;set;} = 0;

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Synergy> Synergies {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}