using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.AuthManagement.ConfirmUserRegistration.Queries;

public record IsUserRegisteredQuery(string email, string token) : IRequest<RequestResult<Guid>>;

public class IsUserRegisteredQueryHandler : BaseRequestHandler<IsUserRegisteredQuery, RequestResult<Guid>>
{
    private readonly IRepository<User> _repository;
    public IsUserRegisteredQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<Guid>> Handle(IsUserRegisteredQuery request, CancellationToken cancellationToken)
    {
        var result= await _repository.Get(u => u.Email == request.email && u.ConfirmationToken == request.token).Select(u => u.ID).FirstOrDefaultAsync();
        if (result == Guid.Empty)
        {
            return RequestResult<Guid>.Failure(ErrorCode.UserNotFound);
        }
        return RequestResult<Guid>.Success(result);
    }
}