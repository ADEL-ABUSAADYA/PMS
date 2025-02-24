// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Infrastructure;
// using PMS.Common;
// using PMS.Common.BaseEndpoints;
// using PMS.Common.Data.Enums;
// using PMS.Common.Views;
// using PMS.Features.UserManagement.GetUserDetalies;
//
//
//
// namespace PMS.Features.UserManagement.GetUserDetails
// {
//   
//     public class UserDetaileEndpoint : BaseEndpoint<RequestUserDetailesEndpoint ,ResponseUserDetailsEndpoint >
//     {
//         public UserDetaileEndpoint(BaseEndpointParameters<RequestUserDetailesEndpoint> parameters) : base(parameters)
//         {
//         }
//
//         [HttpGet]
//         public async Task<EndpointResponse<ResponseUserDetailsEndpoint>> GetUSerDetails([FromQuery] RequestUserDetailesEndpoint request)
//         {
//             var validationResult = ValidateRequest(request);
//             if (!validationResult.isSuccess)
//                 return EndpointResponse<ResponseUserDetailsEndpoint>.Failure(ErrorCode.InvalidInput);
//             
//     
//
//
//
//        
//
//             return EndpointResponse<ResponseUserDetailsEndpoint>.Success(default);
//
//         }
//
//
//     }
// }
