using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Pagination;
using PMS.Models;

namespace PMS.Features.UserManagement.ChangeStatus.Queries;

public record CheckUserActivation(int PageNumber , int PageSize) : IRequest<RequestResult<bool>>;

public class CheckUserActivationHandler : BaseRequestHandler<CheckUserActivation, RequestResult<bool>>
{
    private readonly IRepository<User> _repository;
    public CheckUserActivationHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override Task<RequestResult<bool>> Handle(CheckUserActivation request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}