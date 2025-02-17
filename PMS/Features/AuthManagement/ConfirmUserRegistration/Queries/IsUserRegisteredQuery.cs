using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.AuthManagement.ConfirmUserRegistration.Queries;

public record IsUserRegisteredQuery(string email, string token) : IRequest<RequestResult<int>>;

public class IsUserRegisteredQueryHandler : BaseRequestHandler<IsUserRegisteredQuery, RequestResult<int>>
{
    private readonly IRepository<User> _repository;
    public IsUserRegisteredQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<int>> Handle(IsUserRegisteredQuery request, CancellationToken cancellationToken)
    {
        var result= await _repository.Get(u => u.Email == request.email && u.ConfirmationToken == request.token).Select(u => u.ID).FirstOrDefaultAsync();
        if (result == 0)
        {
            return RequestResult<int>.Failure(ErrorCode.UserNotFound);
        }
        return RequestResult<int>.Success(result);
    }
}