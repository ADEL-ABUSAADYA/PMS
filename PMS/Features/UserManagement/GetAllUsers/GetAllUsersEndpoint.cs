using Microsoft.AspNetCore.Mvc;
using PMS.Common.BaseEndpoints;
using PMS.Common.Views;
using PMS.Features.UserManagement.GetAllUsers.Queries;

namespace PMS.Features.UserManagement.GetAllUsers;

public class GetAllUsersEndpoint : BaseEndpoint<PaginationRequestViewModel ,UserResponseViewModel>
{
   public GetAllUsersEndpoint(BaseEndpointParameters<PaginationRequestViewModel> parameters) : base(parameters)
   {
   }

   [HttpGet]
   public async Task<EndpointResponse<UserResponseViewModel>> GetAllUsers([FromQuery] PaginationRequestViewModel paginationRequest)
   {

        var validationResult = ValidateRequest(paginationRequest);
        if (!validationResult.isSuccess)
            return validationResult;

        var allUsers = await _mediator.Send(new GetAllUsersQuery(paginationRequest.PageNumber, paginationRequest.PageSize));

      if (!allUsers.isSuccess)
         return EndpointResponse<UserResponseViewModel>.Failure(allUsers.errorCode, allUsers.message);

      var paginatedResult = allUsers.data;

      var response = new UserResponseViewModel
      {
         Users = paginatedResult.Items.Select(u => new UserDTO
         {
            Email = u.Email,
            Name = u.Name,
            PhoneNo = u.PhoneNo,
            IsActive = u.IsActive
         }).ToList(),
         TotalCount = paginatedResult.TotalCount,
         PageNumber = paginatedResult.PageNumber,
         PageSize = paginatedResult.PageSize
      };

      return EndpointResponse<UserResponseViewModel>.Success(response);
   }
}
