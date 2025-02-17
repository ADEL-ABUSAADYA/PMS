using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.AuthManagement.ConfirmUserRegistration.Commands;

namespace PMS.Features.AuthManagement.ConfirmUserRegistration;

public class ConfirmEmailEndpoint : BaseEndpoint<ConfirmEmailRequestViewModel, bool>
{
    public ConfirmEmailEndpoint(BaseEndpointParameters<ConfirmEmailRequestViewModel> parameters): base(parameters){}

    [HttpPost]
    public async Task<EndpointResponse<bool>> ConfirmEmail(ConfirmEmailRequestViewModel viewmodel)
    {
        var validationResult =  ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;
        
        var confirmationCommand = new ConfirmEmailCommand(viewmodel.Email, viewmodel.Token);
        var isConfirmed = await _mediator.Send(confirmationCommand);
        if (isConfirmed.isSuccess)
            return EndpointResponse<bool>.Success(isConfirmed.data);

        return EndpointResponse<bool>.Failure(isConfirmed.errorCode, isConfirmed.message);
    }
}

