using MediatR;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using PMS.Common.BaseHandlers;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.ProjectManagement.AddProject.Queries;
using PMS.Models;

namespace PMS.Features.ProjectManagement.AddProject.Commands
{
    public record AddProjectCommand(string Title , string Describtion, DateTime EndDate) : IRequest<RequestResult<bool>>;


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
            var project = await _mediator.Send(new IsProjectExistQuery(request.Title)); 

            if (!project.isSuccess) return RequestResult<bool>.Failure(project.errorCode , project.message);

            var NewProject = new Project
            {
                CreatedBy = _userInfo.ID,
                Description = request.Describtion,
                Title = request.Title,
                CreatedDate = DateTime.Now,
                CreatorID = _userInfo.ID
            }; 

            await _repository.AddAsync(NewProject);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "project created sucssfuly");
        }
    }



}
