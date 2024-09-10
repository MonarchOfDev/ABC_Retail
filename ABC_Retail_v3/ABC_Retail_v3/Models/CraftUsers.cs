using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Retail_v3.Models
{
    public class CraftUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Firstname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "First name should be between 3 and 35 characters")]
        public string EmployeeName { set; get; }

        [Required(ErrorMessage = "Surname is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Surname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Second name should be between 3 and 35 characters")]
        public string EmployeeSurname { set; get; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(255, MinimumLength = 12, ErrorMessage = "Second name should be between 12 and 255 characters")]
        public string Email { get; set; }
    }
}
