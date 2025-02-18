// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using PMS.Common.BaseHandlers;
// using PMS.Common.Data.Enums;
// using PMS.Common.Helper;
// using PMS.Common.Views;
// using PMS.Features.Common.Pagination;
// using PMS.Features.UserManagement.GetAllUsers;
// using PMS.Models;
//
// namespace PMS.Features.ProjectManagement.GetProjectDetailes.Queries
// {
//     public record GetProjectDetailsQuery(int id) : IRequest<RequestResult<GetProjectRequestDTO>>;
//
//     public class GetProjectDetailsQueryHandler : BaseRequestHandler<GetProjectDetailsQuery, RequestResult<GetProjectRequestDTO>, Project>
//     {
//         public GetProjectDetailsQueryHandler(BaseWithoutRepositoryRequestHandlerParameter<Project> parameters) : base(parameters)
//         {
//         }
//
//         public override async Task<RequestResult<GetProjectRequestDTO>> Handle(GetProjectDetailsQuery softDeleteRequest, CancellationToken cancellationToken)
//         {
//             var query = await _repository.Get(c => c.ID == softDeleteRequest.id && !c.Deleted).Select(c => new
//             {
//                 c.SprintItems,
//                 c.CreatedDate,
//                 c.Description,
//                 c.Title
//             }).FirstOrDefaultAsync();
//
//             if (query == null) return RequestResult<GetProjectRequestDTO>.Failure(ErrorCode.ProjectNotFound, "there is no project with this id");
//
//             var project = new GetProjectRequestDTO
//             {
//                 title = query.Title,
//                 description = query.Description,
//                 NumTask = query.SprintItems.Select(c => c.ID).Count(),
//                 NumUSers = query.SprintItems.Select(c => c.UserID).Distinct().Count(),
//                 CreatedDate = query.CreatedDate,
//             };
//
//             return RequestResult<GetProjectRequestDTO>.Success(project);
//         }
//     }
// }
