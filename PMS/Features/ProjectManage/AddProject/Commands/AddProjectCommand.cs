using MediatR;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using PMS.Common.BaseHandlers;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.ProjectManage.AddProject.Queries;
using PMS.Models;

namespace PMS.Features.ProjectManage.AddProject.Commands
{
    public record AddProjectCommand(string title , string describtion, DateTime endDate) : IRequest<RequestResult<bool>>;


    public class AddProjectCommandHandler : BaseRequestHandler<AddProjectCommand, RequestResult<bool>>
    {
        private readonly IRepository<Project> _repository;
        public AddProjectCommandHandler(BaseRequestHandlerParameters parameters, IRepository<Project> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<RequestResult<bool>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        { 

            // check this project is already exist 
            var project = await _mediator.Send(new IsProjectExistQuery(request.title)); 

            if (!project.isSuccess) return RequestResult<bool>.Failure(project.errorCode , project.message);

            var NewProject = new Project
            {
                CreatedBy = _userInfo.ID,
                Description = request.describtion,
                Title = request.title,
                CreatedDate = DateTime.Now,
                CreatorID = _userInfo.ID
            }; 

            await _repository.AddAsync(NewProject);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "project created sucssfuly");
        }
    }



}
