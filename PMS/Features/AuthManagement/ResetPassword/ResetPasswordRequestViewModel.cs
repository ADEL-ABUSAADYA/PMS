using FluentValidation;
using System.ComponentModel.DataAnnotations;
using PMS.Features.AuthManagement.RegisterUser;

namespace PMS.Features.AuthManagement.ResetPassword
{
    public record ResetPasswordRequestViewModel
    {

        public string otp {  get; set; }

        [Required(ErrorMessage = "Password required"), DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{6,}$", 
        ErrorMessage = "Password must be at least 6 characters, include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Doesn't match")]
        public string ConfirmPassword{ get; set; }

    }


    public class ResetPasswordRequestViewModelValidator : AbstractValidator<ResetPasswordRequestViewModel>
    {
        public ResetPasswordRequestViewModelValidator()
        {
         
        }
    }
}