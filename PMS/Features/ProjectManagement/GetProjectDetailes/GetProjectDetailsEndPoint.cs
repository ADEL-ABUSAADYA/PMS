// using Microsoft.AspNetCore.Mvc;
// using PMS.Common.BaseEndpoints;
// using PMS.Common.Helper;
// using PMS.Common.Views;
// using PMS.Features.ProjectManagement.GetProjectDetailes.Queries;
//
//
// namespace PMS.Features.ProjectManagement.GetProjectDetailes
// {
//     public class GetProjectDetailsEndPoint : BaseEndpoint<ProjectDetailsRequest, ProjectDetailsResponseViewModel>
//     {
//         public GetProjectDetailsEndPoint(BaseEndpointParameters<ProjectDetailsRequest> parameters) : base(parameters)
//         {
//         }
//
//         [HttpGet("GetProjectDetails")]
//         public async Task<EndpointResponse<ProjectDetailsResponseViewModel>> GetProjectDetails([FromQuery] ProjectDetailsRequest softDeleteRequest)
//         {
//             var vaildtaion = ValidateRequest(softDeleteRequest);
//
//             if (!vaildtaion.isSuccess) return vaildtaion;
//
//
//
//             var result = await _mediator.Send(new GetProjectDetailsQuery(softDeleteRequest.id));
//             if (!result.isSuccess)
//                 return EndpointResponse<ProjectDetailsResponseViewModel>.Failure(result.errorCode, result.message);
//             var response = new ProjectDetailsResponseViewModel
//             {
//                 title = result.data.title,
//                 description = result.data.description,
//                 NumUSers = result.data.NumUSers,
//                 NumTask = result.data.NumTask,
//                 CreatedDate = result.data.CreatedDate,
//             };
//
//
//
//             return EndpointResponse<ProjectDetailsResponseViewModel>.Success(response);
//         }
//     }
// }
