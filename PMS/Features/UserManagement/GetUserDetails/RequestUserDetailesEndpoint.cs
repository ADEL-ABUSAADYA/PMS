using DotNetCore.CAP;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.AuthManagement.RegisterUser;
using PMS.Features.AuthManagement.RegisterUser.Commands;
using PMS.Features.UserManagement.GetAllUsers;

public class UserDetailsEndpoint : BaseEndpoint<EmptyRequestViewModel, bool>
{
    private readonly ICapPublisher _capPublisher;
    public UserDetailsEndpoint(BaseEndpointParameters<EmptyRequestViewModel> parameters, ICapPublisher capPublisher) : base(parameters)
    {
        _capPublisher = capPublisher;
    }

    [HttpPut]
    public async Task<EndpointResponse<bool>> UserDetails(EmptyRequestViewModel viewmodel)
    {
        var validationResult =  ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;
      
        _capPublisher.PublishAsync("test.message", new { Message = "Hello CAP" });

      
        return EndpointResponse<bool>.Success(true);
    }
}
