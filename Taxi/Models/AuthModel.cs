using System.ComponentModel.DataAnnotations;

namespace Taxi.Models
{
    public class LoginModel
    {
        [Required]
        public string CallSign { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string CallSign { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Name { get; set; }

        [Required] 
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string DateOfBorn { get; set; }

        [Required]
        public string Sex { get; set; }

        public string DriverLicenseId { get; set; }
    }
}