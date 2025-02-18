using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.ProjectManagement.SoftDeleteProject.Command;

public record SoftDeleteListOfSprintItemsCommand(List<int> SprintIDs) : IRequest<RequestResult<bool>>;

public class SoftDeleteListOfSprintItemsCommandHandler : BaseRequestHandler<SoftDeleteListOfSprintItemsCommand, RequestResult<bool>>
{
    private readonly IRepository<SprintItem> _repository;
    public SoftDeleteListOfSprintItemsCommandHandler(BaseRequestHandlerParameters parameters, IRepository<SprintItem> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<bool>> Handle(SoftDeleteListOfSprintItemsCommand request,
        CancellationToken cancellationToken)
    {
        List<SprintItem> sprintItems = new List<SprintItem>();
        foreach (var itemID in request.SprintIDs)
        {
            sprintItems.Add(
                new SprintItem
                {
                    ID = itemID,
                    Deleted = true,
                    UpdatedBy = _userInfo.ID
                }
            );
        }

        foreach (var sprintItem in sprintItems)
        {
            await _repository.SaveIncludeAsync(sprintItem, nameof(SprintItem.Deleted), nameof(SprintItem.UpdatedBy));
        }

        await _repository.SaveChangesAsync();

        return RequestResult<bool>.Success(true);
    }
}