using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.Queries;
using PMS.Models;

namespace PMS.Features.AuthManagement.ResetPassword.Queries;

public record GetUserIDIfPasswordTokenMatchQuery(string Email, string PasswordToken) : IRequest<RequestResult<int>>;

public class GetUserIDIfPasswordTokenMatchQueryHandler : BaseRequestHandler<GetUserIDIfPasswordTokenMatchQuery, RequestResult<int>>
{
    private readonly IRepository<User> _repository;
    public GetUserIDIfPasswordTokenMatchQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<int>> Handle(GetUserIDIfPasswordTokenMatchQuery request, CancellationToken cancellationToken)
    {
        var userID = await _repository.Get(U => U.Email == request.Email && U.ResetPasswordToken == request.PasswordToken).Select(u => u.ID).FirstOrDefaultAsync();

        if(userID <= 0)
            return RequestResult<int>.Failure(ErrorCode.UserNotFound, "User does not exist");
        
        return RequestResult<int>.Success(userID);
    }
}