using FluentValidation;
using PMS.Features.ProjectManage.AddProject;

namespace PMS.Features.ProjectManage.SoftDeleteProject
{
    public record SoftDeleteRequestViewModel(int ProjectID);
    public class RequestEndPointModelValidator : AbstractValidator<SoftDeleteRequestViewModel>
    {
        public RequestEndPointModelValidator()
        {

        }
    }
}