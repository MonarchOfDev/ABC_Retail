using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retail_v3.Models
{
    public class LoginModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}