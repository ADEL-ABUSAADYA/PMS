
using FluentValidation;

public record AddUserFeatureRequestViewModel(string Email, string Password);
public class AddUserFeatureRequestViewModelValidator : AbstractValidator<AddUserFeatureRequestViewModel>
{
    public AddUserFeatureRequestViewModelValidator()
    {
        // RuleFor(x => x.Email)
        //     .NotEmpty().WithMessage("Email is required.")
        //     .EmailAddress().WithMessage("Please provide a valid email address.");
        //
        // RuleFor(x => x.Password)
        //     .NotEmpty().WithMessage("Password is required.")
        //     .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        //
        // RuleFor(x => x.Name)
        //     .NotEmpty().WithMessage("Name is required.");
        //
        // RuleFor(x => x.PhoneNo)
        //     .NotEmpty().WithMessage("Phone number is required.")
        //     .Matches(@"^\+?\d{10,15}$").WithMessage("Please provide a valid phone number.");
        //
        // RuleFor(x => x.Country)
        //     .NotEmpty().WithMessage("Country is required.");
    }
}