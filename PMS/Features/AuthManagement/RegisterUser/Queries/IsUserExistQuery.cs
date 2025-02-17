using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.DTOs;
using PMS.Models;

namespace PMS.Features.AuthManagement.RegisterUser.Queries;

public record IsUserExistQuery (string email) : IRequest<RequestResult<bool>>;


public class IsUserExistQueryHandler : BaseRequestHandler<IsUserExistQuery, RequestResult<bool>>
{
    private readonly IRepository<User> _repository;
    public IsUserExistQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<bool>> Handle(IsUserExistQuery request, CancellationToken cancellationToken)
    {
        var result= await _repository.AnyAsync(u => u.Email == request.email);
        if (!result)
        {
            return RequestResult<bool>.Failure(ErrorCode.UserNotFound);
        }

        return RequestResult<bool>.Success(result);
    }
}