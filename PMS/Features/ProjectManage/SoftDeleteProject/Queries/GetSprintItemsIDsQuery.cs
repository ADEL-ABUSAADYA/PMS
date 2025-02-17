using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.ProjectManage.SoftDeleteProject.Queries;

public record GetSprintItemsIDsQuery(int ProjectID) : IRequest<RequestResult<List<int>>>;

public class GetSprintItemsIDsQueryHandler : BaseRequestHandler<GetSprintItemsIDsQuery, RequestResult<List<int>>>
{
    private readonly IRepository<SprintItem> _repository;
    public GetSprintItemsIDsQueryHandler(BaseRequestHandlerParameters parameters, IRepository<SprintItem> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<List<int>>> Handle(GetSprintItemsIDsQuery request, CancellationToken cancellationToken)
    {
        var sprintItemsIDs = await _repository.Get(si => si.ProjectID == request.ProjectID && si.Deleted == false).Select(si => si.ID).ToListAsync();
        if (sprintItemsIDs.Count <= 0)
            return RequestResult<List<int>>.Failure(ErrorCode.NoSprintItems, "No sprint items found");
        
        return RequestResult<List<int>>.Success(sprintItemsIDs);
    }
}