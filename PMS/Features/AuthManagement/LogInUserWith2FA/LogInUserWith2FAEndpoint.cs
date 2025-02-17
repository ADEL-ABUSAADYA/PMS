using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.AuthManagement.LogInUserWith2FA.Commands;

namespace PMS.Features.AuthManagement.LogInUserWith2FA;

public class LogInUserWith2FAEndpoint : BaseEndpoint<LogInUserWith2FARequestViewModel, string>
{
   public LogInUserWith2FAEndpoint(BaseEndpointParameters<LogInUserWith2FARequestViewModel> parameters) : base(parameters)
   {
   }

   [HttpPost]
   [Authorize(AuthenticationSchemes = "2FA")]
   public async Task<EndpointResponse<string>> LogInUserWith2FA(LogInUserWith2FARequestViewModel viewmodel)
   {
      var validationResult =  ValidateRequest(viewmodel);
      if (!validationResult.isSuccess)
         return validationResult;
      
      var loginWith2FACommand = new LogInUserWith2FACommand(viewmodel.Email, viewmodel.Otp);
      var logInToken = await _mediator.Send(loginWith2FACommand);
      if (!logInToken.isSuccess)
         return EndpointResponse<string>.Failure(logInToken.errorCode, logInToken.message);
      
      return EndpointResponse<string>.Success(logInToken.data);
   }
}
