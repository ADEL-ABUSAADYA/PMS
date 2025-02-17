using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.Common.Users.DTOs;
using PMS.Features.AuthManagement.LogInUser.Commands;
using PMS.Features.AuthManagement.RegisterUser.Commands;
using PMS.Features.AuthManagement.ReSendRegistrationEmail.Commands;

namespace PMS.Features.AuthManagement.LogInUser;

public class LogInUserEndpoint : BaseEndpoint<LogInUserRequestViewModel, LoginResponeViewModel>
{
   public LogInUserEndpoint(BaseEndpointParameters<LogInUserRequestViewModel> parameters) : base(parameters)
   {
   }

   [HttpPost]
   public async Task<EndpointResponse<LoginResponeViewModel>> LogInUser(LogInUserRequestViewModel viewmodel)
   {
      var validationResult =  ValidateRequest(viewmodel);
      if (!validationResult.isSuccess)
         return validationResult;
      
      var loginCommand = new LogInUserCommand(viewmodel.Email, viewmodel.Password);
      var logInToken = await _mediator.Send(loginCommand);
      if (!logInToken.isSuccess)
         return EndpointResponse<LoginResponeViewModel>.Failure(logInToken.errorCode, logInToken.message);
      
      return EndpointResponse<LoginResponeViewModel>.Success(new LoginResponeViewModel(Token: logInToken.data.LogInToken, TokenWith2FA: logInToken.data.TokenWith2FA));
   }
}
