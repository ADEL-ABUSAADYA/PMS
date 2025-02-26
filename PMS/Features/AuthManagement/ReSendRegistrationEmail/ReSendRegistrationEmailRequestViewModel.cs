using FluentValidation;

namespace PMS.Features.AuthManagement.ResendRegistrationEmail;

public record ReSendRegistrationEmailRequestViewModel(string Email);

public class RegisterUserRequestViewModelValidator : AbstractValidator<ReSendRegistrationEmailRequestViewModel>
{
    public RegisterUserRequestViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.");
    }
}