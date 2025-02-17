using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.ProjectManage.AddProject.Queries
{
    public record IsProjectExistQuery(string title) : IRequest<RequestResult<bool>>;


    public class IsProjectExistQueryHandler : BaseRequestHandler<IsProjectExistQuery, RequestResult<bool>>
    {
        private readonly IRepository<Project> _repository;
        public IsProjectExistQueryHandler(BaseRequestHandlerParameters parameters, IRepository<Project> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<RequestResult<bool>> Handle(IsProjectExistQuery request, CancellationToken cancellationToken)
        {
            var projectExist = await _repository.AnyAsync(p=> p.Title == request.title);

            if (projectExist) return RequestResult<bool>.Failure(ErrorCode.ProjectAlreadyExists, "this project is already exist"); 

           
            return RequestResult<bool>.Success(projectExist);   

        }
    }
}
