using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using PMS.Common.BaseEndpoints;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.AuthManagement.ActivateUser2FA.Commands;
using PMS.Features.AuthManagement.LogInUser;
using PMS.Filters;

namespace PMS.Features.AuthManagement.ActivateUser2FA;


public class Activate2FAQRCodeEndpoint : BaseEndpoint<EmptyRequestViewModel, string>
{
    public Activate2FAQRCodeEndpoint(BaseEndpointParameters<EmptyRequestViewModel> parameters) : base(parameters)
    {
    }
    
    [HttpPost]
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments =new object[] {Feature.ActivateUser2FA})]
    public async Task<EndpointResponse<string>> ActivateUser2FA()
    {
        var activateCommand = await _mediator.Send(new ActivateUser2FAOrchestrator());
        if (!activateCommand.isSuccess)
            return EndpointResponse<string>.Failure(activateCommand.errorCode, activateCommand.message);
        
        return EndpointResponse<string>.Success(activateCommand.data);
    }
}