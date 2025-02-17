using FluentValidation;
using PMS.Features.AuthManagement.RegisterUser;

namespace PMS.Features.AuthManagement.SendFrogetPasswordResetEmail
{
    public record ForgetPasswordViewModel(string email); 


     public class ForgetPasswordViewModelValidator : AbstractValidator<ForgetPasswordViewModel>
    {
        public ForgetPasswordViewModelValidator ()
        {
         
        }
    }

}