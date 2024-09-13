using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Scripting;
using Org.BouncyCastle.Crypto.Generators;
using System.Web.Helpers;
using Azure.Data.Tables;

namespace ABC_Retail_v3.Models
{
    public class CraftUsers : ITableEntity
    {
        // Implement PartitionKey and RowKey
        public string? PartitionKey { get; set; }
        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Firstname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "First name should be between 3 and 35 characters")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Surname is requred")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a Surname, No Numbers allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Second name should be between 3 and 35 characters")]
        public string Surname { set; get; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(255, MinimumLength = 12, ErrorMessage = "Second name should be between 12 and 255 characters")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public UserRole Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public CraftUsers()
        {
            // Example: Partition by Category and RowKey by ProductId or a unique identifier
            PartitionKey = Email;
            RowKey = Guid.NewGuid().ToString();
        }

        public void ValidateUser()
        {
            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
                throw new ArgumentException("Email is invalid.");

            if (!Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"))
                throw new ArgumentException("Password must contain uppercase, lowercase, number, and special character.");
        }

        public enum UserRole
        {
            Employee,
            User,
            Customer,
            Guest
        }
    }
}
