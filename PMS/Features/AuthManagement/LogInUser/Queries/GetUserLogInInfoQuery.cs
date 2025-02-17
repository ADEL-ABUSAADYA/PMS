using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.LogInUser;
using PMS.Models;

namespace PMS.Features.AuthManagement.LogInUser.Queries;
public record GetUserLogInInfoQuery(string email) : IRequest<RequestResult<LogInInfoDTO>>;

public class GetUserLogInInfoQueryHandler : BaseRequestHandler<GetUserLogInInfoQuery, RequestResult<LogInInfoDTO>>
{
    private readonly IRepository<User> _repository;
    public GetUserLogInInfoQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<LogInInfoDTO>> Handle(GetUserLogInInfoQuery request, CancellationToken cancellationToken)
    {
        var userData  = await _repository.Get(u => u.Email == request.email && u.IsActive == true)
            .Select(u => new LogInInfoDTO(u.ID, u.TwoFactorAuthEnabled, u.Password, u.IsEmailConfirmed)).FirstOrDefaultAsync();
        
        if (userData.ID <= 0)
        {
            return RequestResult<LogInInfoDTO>.Failure(ErrorCode.UserNotFound, "please check your email address.");
        }
        return RequestResult<LogInInfoDTO>.Success(userData);
    }
}