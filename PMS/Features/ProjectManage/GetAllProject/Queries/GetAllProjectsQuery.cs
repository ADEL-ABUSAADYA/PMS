using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Pagination;
using PMS.Models;

namespace PMS.Features.ProjectManage.GetAllProject.Queries
{
    public record GetAllProjectsQuery(int PageNumber, int PageSize) : IRequest<RequestResult<PaginatedResult<ProjectDTO>>>;

    public class GetAllProjectsQueryHandler : BaseRequestHandler<GetAllProjectsQuery, RequestResult<PaginatedResult<ProjectDTO>>>
    {
        private readonly IRepository<Project> _repository;
        public GetAllProjectsQueryHandler(BaseRequestHandlerParameters parameters, IRepository<Project> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<RequestResult<PaginatedResult<ProjectDTO>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetAll();

            if (request.PageNumber < 1) return RequestResult<PaginatedResult<ProjectDTO>>.Failure(ErrorCode.NoProjectAdd, "there is no project add");

            if (request.PageSize < 1 || request.PageSize > 100)
                return RequestResult<PaginatedResult<ProjectDTO>>.Failure(ErrorCode.InvalidInput, "PageSize must be between 1 and 100");

            int totalProjects = await query.CountAsync(cancellationToken);
            
            var projects = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProjectDTO
                {
                    Title = p.Title,
                    Description = p.Description,
                    Status = p.Status,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    CreatorName = p.Creator.Name
                })
                .ToListAsync(cancellationToken);

            if (!projects.Any())
                return RequestResult<PaginatedResult<ProjectDTO>>.Failure(ErrorCode.NoUsersFound, "No users found");

            // Create the PaginatedResult
            var paginatedResult = new PaginatedResult<ProjectDTO>(projects, totalProjects, request.PageNumber, request.PageSize);

            return RequestResult<PaginatedResult<ProjectDTO>>.Success(paginatedResult);
        }
    }
}
