using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.ProjectManagement.SoftDeleteProject.Command;
using PMS.Filters;

namespace PMS.Features.ProjectManagement.SoftDeleteProject
{
    public class SoftDeleteProjectEndpoint : BaseEndpoint<SoftDeleteRequestViewModel, bool>
    {
        public SoftDeleteProjectEndpoint(BaseEndpointParameters<SoftDeleteRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpPut]
        [Authorize]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments =new object[] {Feature.DeleteProject})]
        
        public async Task<EndpointResponse<bool>> SoftDeletProject([FromQuery]SoftDeleteRequestViewModel softDeleteRequest)
        {
            var vailtion = ValidateRequest(softDeleteRequest);
            if (!vailtion.isSuccess) return vailtion; 

            var response = await _mediator.Send(new SoftDeleteProjectOrchestrator(softDeleteRequest.ProjectID)); 

            if(!response.isSuccess) return EndpointResponse<bool>.Failure(response.errorCode,response.message);

            return EndpointResponse<bool>.Success(true, "project is soft deleted");

        }
    }
}
