using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.Common.Users.Queries;

public record GetUserIDByEmailQuery(string Email) : IRequest<RequestResult<Guid>>;

public class GetUserIDByEmailQueryHandler : BaseRequestHandler<GetUserIDByEmailQuery, RequestResult<Guid>>
{
    private readonly IRepository<User> _repository;
    public GetUserIDByEmailQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<Guid>> Handle(GetUserIDByEmailQuery request, CancellationToken cancellationToken)
    {
        var userID = await _repository.Get(U => U.Email == request.Email).Select(u => u.ID).FirstOrDefaultAsync();

        if(userID == Guid.Empty)
            return RequestResult<Guid>.Failure(ErrorCode.UserNotFound, "User does not exist");
        
        return RequestResult<Guid>.Success(userID);
    }
}