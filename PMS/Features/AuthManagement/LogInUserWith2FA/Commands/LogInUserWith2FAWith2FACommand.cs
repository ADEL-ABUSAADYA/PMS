using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.AuthManagement.LogInUser.Queries;
using PMS.Models;
using OtpNet;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.LogInUserWith2FA.Queries;

namespace PMS.Features.AuthManagement.LogInUserWith2FA.Commands;

public record LogInUserWith2FACommand(string Email, string Otp) : IRequest<RequestResult<string>>;

public class LogInUserWith2FACommandHandler : BaseRequestHandler<LogInUserWith2FACommand, RequestResult<string>>
{
    private readonly IRepository<User> _repository;
    public LogInUserWith2FACommandHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<string>> Handle(LogInUserWith2FACommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _mediator.Send(new GetUserLogInInfoWith2FAQuery(request.Email));
        if (!userInfo.isSuccess)
        {
            return RequestResult<string>.Failure(userInfo.errorCode, userInfo.message);
        }

        var user = userInfo.data;
        
        if (user.ID != Guid.Empty || !user.Is2FAEnabled || string.IsNullOrEmpty(user.TwoFactorAuthsecretKey))
        {
            return RequestResult<string>.Failure(ErrorCode.UserNotFound, "User does not exist or 2FA is not enabled.");
        }

        // Validate the OTP using the secret key stored for the user
        var isOtpValid = Validate2FACode(user.TwoFactorAuthsecretKey, request.Otp);
        if (!isOtpValid)
        {
            return RequestResult<string>.Failure(ErrorCode.Invalid2FA, "Invalid OTP.");
        }

        // OTP is valid, issue a full JWT token for authentication
        var fullAccessToken = _tokenHelper.GenerateToken(user.ID);
        return RequestResult<string>.Success(fullAccessToken);
    }

    private bool Validate2FACode(string userSecretKey, string enteredOtp)
    {
        var keyBytes = Base32Encoding.ToBytes(userSecretKey);
        var totp = new Totp(keyBytes);
        
        return totp.VerifyTotp(enteredOtp, out long timeStepMatched);
    }
}