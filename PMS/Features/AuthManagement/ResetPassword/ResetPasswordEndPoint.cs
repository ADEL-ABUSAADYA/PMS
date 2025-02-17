using Azure;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using PMS.Common.BaseEndpoints;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Features.AuthManagement.ResetPassword.Commands;

namespace PMS.Features.AuthManagement.ResetPassword
{
    public class ResetPasswordEndPoint : BaseEndpoint<ResetPasswordRequestViewModel, bool>
    {
        public ResetPasswordEndPoint(BaseEndpointParameters<ResetPasswordRequestViewModel> parameters) : base(parameters)
        {
        }


        [HttpPost]
         
        public async Task<EndpointResponse<bool>> ResetPassword(ResetPasswordRequestViewModel parameters)
        {
            var validateInput = ValidateRequest(parameters);
            if (!validateInput.isSuccess) return EndpointResponse<bool>.Failure(ErrorCode.InvalidInput);

            var response = await _mediator.Send(new ResetPasswordCommand(parameters.otp, parameters.Email, parameters.Email));

            if (!response.isSuccess) return EndpointResponse<bool>.Failure(response.errorCode, response.message);

            return EndpointResponse<bool>.Success(true, "password change sucssfuly");
        }

    }
}
