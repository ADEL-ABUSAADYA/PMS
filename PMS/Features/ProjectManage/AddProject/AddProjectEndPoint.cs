using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
using PMS.Common.BaseEndpoints;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.AuthManagement.RegisterUser.Commands;
using PMS.Features.ProjectManage.AddProject.Commands;
using PMS.Filters;
using PMS.Models;

namespace PMS.Features.ProjectManage.AddProject
{
    public class AddProjectEndPoint : BaseEndpoint<RequestAddProjectModel, bool>
    {
        public AddProjectEndPoint(BaseEndpointParameters<RequestAddProjectModel> parameters) : base(parameters)
        {
        }

        [HttpPost]
        [Authorize]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments =new object[] {Feature.AddProject})]
        public async Task<EndpointResponse<bool>> AddBook(RequestAddProjectModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;

            var Project = await _mediator.Send(new AddProjectCommand (viewmodel.Title , viewmodel.Descrbition, viewmodel.EndDate));
            if (!Project.isSuccess)
                return EndpointResponse<bool>.Failure(Project.errorCode, Project.message);

            return EndpointResponse<bool>.Success(true);
        }


    }
}
