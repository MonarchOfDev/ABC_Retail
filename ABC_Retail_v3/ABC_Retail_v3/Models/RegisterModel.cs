using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retail_v3.Models
{

    public class RegisterModel
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}