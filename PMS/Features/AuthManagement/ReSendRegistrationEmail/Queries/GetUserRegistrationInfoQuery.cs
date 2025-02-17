using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.ResendRegistrationEmail;
using PMS.Models;

namespace PMS.Features.AuthManagement.ReSendRegistrationEmail.Queries;

public record GetUserRegistrationInfoQuery(string email) : IRequest<RequestResult<RegistrationInfoDTO>>;

public class GetUserRegistrationInfoQueryHandler : BaseRequestHandler<GetUserRegistrationInfoQuery, RequestResult<RegistrationInfoDTO>>
{
    private readonly IRepository<User> _repository;
    public GetUserRegistrationInfoQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }


    public override async Task<RequestResult<RegistrationInfoDTO>> Handle(GetUserRegistrationInfoQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.Get(u => u.Email == request.email).Select(u => new RegistrationInfoDTO
        {
            Name = u.Name,
            ConfirmationToken = u.ConfirmationToken,
            Email = u.Email,
            IsRegistered = u.IsEmailConfirmed
        }).FirstOrDefaultAsync();
        
        if (result == null)
            return RequestResult<RegistrationInfoDTO>.Failure(ErrorCode.UserNotFound, "please check your email address or register your email address.");
        
        return RequestResult<RegistrationInfoDTO>.Success(result);
    }
}