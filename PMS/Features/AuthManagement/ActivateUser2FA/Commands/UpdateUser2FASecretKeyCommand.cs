using MediatR;
using Microsoft.AspNetCore.Identity;
using OtpNet;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.Queries;
using PMS.Models;


public record UpdateUser2FASecretKeyCommand(string User2FASecretKey) : IRequest<RequestResult<bool>>;

public class UpdateUser2FASecretKeyCommandHandler : BaseRequestHandler<UpdateUser2FASecretKeyCommand, RequestResult<bool>>
{
    private readonly IRepository<User> _repository;
    public UpdateUser2FASecretKeyCommandHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<bool>> Handle(UpdateUser2FASecretKeyCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            ID = _userInfo.ID,
            TwoFactorAuthsecretKey = request.User2FASecretKey,
            TwoFactorAuthEnabled = true,
        };

        try
        {

            _repository.SaveIncludeAsync(user, nameof(user.TwoFactorAuthsecretKey), nameof(user.TwoFactorAuthEnabled));
            _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return RequestResult<bool>.Failure(ErrorCode.UnKnownError, e.Message);
        }

        return RequestResult<bool>.Success(true);
    }
}