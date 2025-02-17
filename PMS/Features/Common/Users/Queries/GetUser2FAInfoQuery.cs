using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.DTOs;
using PMS.Models;


namespace PMS.Features.Common.Users.Queries;
public record GetUser2FAInfoQuery() : IRequest<RequestResult<User2FAInfoDTO>>;

public class GetUser2FAInfoQueryHandler : BaseRequestHandler<GetUser2FAInfoQuery, RequestResult<User2FAInfoDTO>>
{
    private readonly IRepository<User> _repository;
    public GetUser2FAInfoQueryHandler (BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<User2FAInfoDTO>> Handle(GetUser2FAInfoQuery request, CancellationToken cancellationToken)
    {
        var userData  = await _repository.Get(u => u.ID == _userInfo.ID)
            .Select(u => new User2FAInfoDTO(u.TwoFactorAuthEnabled, u.TwoFactorAuthsecretKey)).FirstOrDefaultAsync();
        
        if (userData is null)
        {
            return RequestResult<User2FAInfoDTO>.Failure(ErrorCode.UserNotFound, "please login again");
        }
        return RequestResult<User2FAInfoDTO>.Success(userData);
    }
}