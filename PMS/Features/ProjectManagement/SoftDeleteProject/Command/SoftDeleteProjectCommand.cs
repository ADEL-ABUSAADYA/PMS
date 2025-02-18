using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;


namespace PMS.Features.ProjectManagement.SoftDeleteProject.Command
{
    public record SoftDeleteProjectCommand(int ProjectID) : IRequest<RequestResult<bool>>;

    public class DeleteProjectCommandHandler : BaseRequestHandler<SoftDeleteProjectCommand, RequestResult<bool>>
    {
        private readonly IRepository<Project> _repository;
        public DeleteProjectCommandHandler(BaseRequestHandlerParameters parameters, IRepository<Project> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<RequestResult<bool>> Handle(SoftDeleteProjectCommand request, CancellationToken cancellationToken)
        {
            // var project = await _repository.Get(c => c.ID == softDeleteRequest.ProjectId && !c.Deleted).FirstOrDefaultAsync(); 

          

            //foreach (var item in project.SprintItems)
            //{

            //    item.Deleted = true;

            //}

            
            // //await _repository.SaveIncludeAsync(project , nameof(project.SprintItems));
            // await _repository.Delete(project);
            // await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "project deleted succssfully ");
        }
    }

}
