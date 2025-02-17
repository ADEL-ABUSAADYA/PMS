using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using PMS.Common.BaseEndpoints;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.Common.Users.Queries;
using PMS.Filters;

namespace PMS.Features.AuthManagement.GetUser2FALink;

public class Get2FAQRCodeEndpoint : BaseEndpoint<EmptyRequestViewModel, string>
{
    public Get2FAQRCodeEndpoint(BaseEndpointParameters<EmptyRequestViewModel> parameters) : base(parameters)
    {
    }
    [HttpGet]
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments =new object[] {Feature.GetUser2FALink})]
    public  async Task<EndpointResponse<string>> GetLink()
    {
        var user2FADataCommand =await _mediator.Send(new GetUser2FAInfoQuery());
        if (!user2FADataCommand.isSuccess)
            return EndpointResponse<string>.Failure(user2FADataCommand.errorCode, user2FADataCommand.message);

        if (!user2FADataCommand.data.Is2FAEnabled || user2FADataCommand.data.TwoFactorAuthSecretKey is null)
            return EndpointResponse<string>.Failure(ErrorCode.Uasr2FAIsNotEnabled, "please activate 2FA first");
        
        var appName = "UpSkilling-PMS";
        string otpUrl = $"otpauth://totp/{appName}:{_userInfo.ID}?secret={user2FADataCommand.data.TwoFactorAuthSecretKey}&issuer={appName}";

        return EndpointResponse<string>.Success(otpUrl);
    }
}