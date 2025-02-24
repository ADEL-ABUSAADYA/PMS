using FluentValidation;
using PMS.Features.ProjectManagement.AddProject;

namespace PMS.Features.ProjectManagement.SoftDeleteProject
{
    public record SoftDeleteRequestViewModel(Guid ProjectID);
    public class RequestEndPointModelValidator : AbstractValidator<SoftDeleteRequestViewModel>
    {
        public RequestEndPointModelValidator()
        {

        }
    }
}