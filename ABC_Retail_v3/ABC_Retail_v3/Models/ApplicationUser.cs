using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retail_v3.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Firstname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "First name should be between 3 and 35 characters")]
        public string Firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Surname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Second name should be between 3 and 35 characters")]
        public string Lastname { get; set; }

        //
        //
        //
        //
        //
        public string UserRole { get; set; }


    }
}
