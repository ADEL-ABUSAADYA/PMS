using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.Queries;
using PMS.Models;

namespace PMS.Features.AuthManagement.ResetPassword.Queries;

public record GetUserIDIfPasswordTokenMatchQuery(string Email, string PasswordToken) : IRequest<RequestResult<Guid>>;

public class GetUserIDIfPasswordTokenMatchQueryHandler : BaseRequestHandler<GetUserIDIfPasswordTokenMatchQuery, RequestResult<Guid>>
{
    private readonly IRepository<User> _repository;
    public GetUserIDIfPasswordTokenMatchQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<Guid>> Handle(GetUserIDIfPasswordTokenMatchQuery request, CancellationToken cancellationToken)
    {
        var userID = await _repository.Get(U => U.Email == request.Email && U.ResetPasswordToken == request.PasswordToken).Select(u => u.ID).FirstOrDefaultAsync();

        if(userID == Guid.Empty)
            return RequestResult<Guid>.Failure(ErrorCode.UserNotFound, "User does not exist");
        
        return RequestResult<Guid>.Success(userID);
    }
}