using FluentValidation;

namespace PMS.Common.Views;

public record EmptyRequestViewModel();

public class LogInUserRequestViewModelValidator : AbstractValidator<EmptyRequestViewModel>
{
    public LogInUserRequestViewModelValidator()
    {
        
    }
}