using Azure;
using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;


namespace PMS.Features.SprintItemManagement.AddSprintItem.Command
{
    public record AddSprintItemCommand(string Title , string Description ) :IRequest<RequestResult<bool>>
    {
    }
    public class AddTaskCommandHandler : BaseRequestHandler<AddSprintItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<SprintItem> _repository;
        public AddTaskCommandHandler(BaseRequestHandlerParameters parameters, IRepository<SprintItem> repository) : base(parameters)
        {
            _repository = repository;
        }

        public async override Task<RequestResult<bool>> Handle(AddSprintItemCommand request, CancellationToken cancellationToken)
        {
            var task = new SprintItem
            {
                Title = request.Title,
                Description = request.Description,
            };
            await _repository.AddAsync(task);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Task Added Successfully");

        }

    }
}
