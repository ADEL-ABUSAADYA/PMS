using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.AuthManagement.SendFrogetPasswordResetEmail.Commands;

namespace PMS.Features.AuthManagement.SendFrogetPasswordResetEmail
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendFrogetPasswordResstEmailEnpoint : BaseEndpoint<ForgetPasswordViewModel, bool>
    {
        public SendFrogetPasswordResstEmailEnpoint(BaseEndpointParameters<ForgetPasswordViewModel> parameters) : base(parameters)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<bool>> SendFrogetPasswordResstEmail( [FromQuery]ForgetPasswordViewModel model)
        {
            var validationResult = ValidateRequest(model);
            if (!validationResult.isSuccess)
                return EndpointResponse<bool>.Failure(ErrorCode.InvalidInput);
            
            var response = await _mediator.Send(new SendForgetPasswordResetEmailCommand(model.email));

            if(!response.isSuccess)
                return EndpointResponse<bool>.Failure(response.errorCode , response.message);

            return EndpointResponse<bool>.Success(true);

        }


    }
}
