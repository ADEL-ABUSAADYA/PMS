using FluentValidation;
using PMS.Features.ProjectManagement.AddProject;

namespace PMS.Features.ProjectManagement.SoftDeleteProject
{
    public record SoftDeleteRequestViewModel(int ProjectID);
    public class RequestEndPointModelValidator : AbstractValidator<SoftDeleteRequestViewModel>
    {
        public RequestEndPointModelValidator()
        {

        }
    }
}