using FluentValidation;
using PMS.Features.ProjectManage.GetAllProject;

namespace PMS.Features.ProjectManage.GetProjectDetailes
{
    public record ProjectDetailsRequest(int id);

    public class ProjectDetailsRequestValidator : AbstractValidator<ProjectDetailsRequest>
    {
        public ProjectDetailsRequestValidator()
        {

        }
    }


}