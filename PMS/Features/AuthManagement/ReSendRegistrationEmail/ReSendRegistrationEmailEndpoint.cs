using System.Text.Json;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.AuthManagement.RegisterUser;
using PMS.Features.AuthManagement.ReSendRegistrationEmail.Queries;

namespace PMS.Features.AuthManagement.ResendRegistrationEmail;

public class ReSendRegistrationEmailEndpoint : BaseEndpoint<ReSendRegistrationEmailRequestViewModel, bool>
{
   private readonly ICapPublisher _capPublisher;
   public ReSendRegistrationEmailEndpoint(BaseEndpointParameters<ReSendRegistrationEmailRequestViewModel> parameters, ICapPublisher capPublisher) : base(parameters)
   {
      _capPublisher = capPublisher;
   }

   [HttpPut]
   public async Task<EndpointResponse<bool>> ReSendEmail(ReSendRegistrationEmailRequestViewModel viewmodel)
   {
      var validationResult =  ValidateRequest(viewmodel);
      if (!validationResult.isSuccess)
         return validationResult;
      
      var registrationInfoQuery = new GetUserRegistrationInfoQuery(viewmodel.Email);
      
      var regisRequestInfo = await _mediator.Send(registrationInfoQuery);
      if (!regisRequestInfo.isSuccess)
         return EndpointResponse<bool>.Failure(regisRequestInfo.errorCode, regisRequestInfo.message);
      
      var message = new UserRegisteredEvent(regisRequestInfo.data.Email, regisRequestInfo.data.Name, $"{regisRequestInfo.data.ConfirmationToken}", DateTime.UtcNow);
      var messageJson = JsonSerializer.Serialize(message);
      await _capPublisher.PublishAsync("user.registered", messageJson);
      
      return EndpointResponse<bool>.Success(true);
   }
}
