using FluentValidation;

namespace PMS.Features.UserManagement.GetAllUsers;

public record PaginationRequestViewModel(int PageNumber, int PageSize);

public class PaginationRequestViewModelValidator : AbstractValidator<PaginationRequestViewModel>
{
    public PaginationRequestViewModelValidator()
    {
        // RuleFor(x => x.PageNumber)
        //     .GreaterThanOrEqualTo(1)
        //     .WithMessage("PageNumber must be greater than or equal to 1.");
        //
        // RuleFor(x => x.PageSize)
        //     .GreaterThanOrEqualTo(1)
        //     .LessThanOrEqualTo(100)
        //     .WithMessage("PageSize must be between 1 and 100.");
    }
}