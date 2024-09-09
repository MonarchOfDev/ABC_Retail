using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ABC_Retail_v3.Models
{
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Customer_id { set; get; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Firstname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "First name should be between 3 and 35 characters")]
        public string First_name { set; get; }

        [Required(ErrorMessage = "Surname is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Surname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Second name should be between 3 and 35 characters")]
        public string Surname { set; get; }

        [StringLength(100, MinimumLength = 17, ErrorMessage = "Email should be between 17 and 100 characters")]
        [Required(ErrorMessage = "Email is requred")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public DateTime? Creation_date { set; get; } = DateTime.Now;

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
