using System.ComponentModel.DataAnnotations;

namespace Dvor.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Full name length must be between 2 and 25 symbols")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}