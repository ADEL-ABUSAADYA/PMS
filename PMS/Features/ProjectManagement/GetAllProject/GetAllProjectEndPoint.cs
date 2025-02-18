using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.ProjectManagement.GetAllProject.Queries;
using PMS.Features.UserManagement.GetAllUsers;

namespace PMS.Features.ProjectManagement.GetAllProject
{
    public class GetAllProjectEndPoint : BaseEndpoint<PaginationRequestViewModel, ProjectResponseViewModel>
    {
        public GetAllProjectEndPoint(BaseEndpointParameters<PaginationRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<ProjectResponseViewModel>> GetAllPeoject(PaginationRequestViewModel paginationRequest)
        {
            var validationResult = ValidateRequest(paginationRequest);
            if (!validationResult.isSuccess)
                return validationResult;

            var AllProject = await _mediator.Send(new GetAllProjectsQuery(paginationRequest.PageNumber, paginationRequest.PageSize));

            if (!AllProject.isSuccess)
                return EndpointResponse<ProjectResponseViewModel>.Failure(AllProject.errorCode, AllProject.message);
            
            var paginatedResult = AllProject.data;
            
            var response = new ProjectResponseViewModel
            {
                Projects = paginatedResult.Items.Select(U => new ProjectDTO
                {
                    Title = U.Title,
                    Description = U.Description,
                    StartDate = U.StartDate,
                    Status = U.Status,
                    EndDate = U.EndDate,
                    CreatorName = U.CreatorName

                }).ToList(),
                PageNumber = paginatedResult.PageNumber,
                PageSize = paginatedResult.PageSize,
                totalNumber = paginatedResult.TotalCount
            };
            return EndpointResponse<ProjectResponseViewModel>.Success(response);
        }


    }
}
