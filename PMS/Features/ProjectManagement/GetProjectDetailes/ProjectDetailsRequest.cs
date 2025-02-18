using FluentValidation;
using PMS.Features.ProjectManagement.GetAllProject;

namespace PMS.Features.ProjectManagement.GetProjectDetailes
{
    public record ProjectDetailsRequest(int id);

    public class ProjectDetailsRequestValidator : AbstractValidator<ProjectDetailsRequest>
    {
        public ProjectDetailsRequestValidator()
        {

        }
    }


}