using System.ComponentModel.DataAnnotations;

namespace Intelliface.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "E-mail can not be null")]
        [EmailAddress(ErrorMessage = "Wrong format")]
        public required string EMail { get; set; }

        [Required(ErrorMessage = "Password can not be null")]
        [MinLength(10, ErrorMessage = "Minimum length is 10")]
        public required string Password { get; set; }
    }
}
