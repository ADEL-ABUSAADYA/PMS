using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.AuthManagement.RegisterUser.Commands;

namespace PMS.Features.AuthManagement.RegisterUser;

public class RegisterUserEndpoint : BaseEndpoint<RegisterUserRequestViewModel, bool>
{
   public RegisterUserEndpoint(BaseEndpointParameters<RegisterUserRequestViewModel> parameters) : base(parameters)
   {
   }

   [HttpPut]
   public async Task<EndpointResponse<bool>> RegisterUser(RegisterUserRequestViewModel viewmodel)
   {
      var validationResult =  ValidateRequest(viewmodel);
      if (!validationResult.isSuccess)
         return validationResult;
      
      var regisetrCommand = new RegisterUserCommand(viewmodel.Email, viewmodel.Password, viewmodel.Name, viewmodel.PhoneNo, viewmodel.Country);
      var isRegistered = await _mediator.Send(regisetrCommand);
      if (!isRegistered.isSuccess)
         return EndpointResponse<bool>.Failure(isRegistered.errorCode, isRegistered.message);
      
      return EndpointResponse<bool>.Success(true);
   }
}
