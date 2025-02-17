using FluentValidation;
using PMS.Features.UserManagement.GetAllUsers;

namespace PMS.Features.UserManagement.GetUserDetalies
{
    public record RequestUserDetailesEndpoint(int id);

    public class RequestUserDetailesEndpointValidator : AbstractValidator<RequestUserDetailesEndpoint>
    {
        public RequestUserDetailesEndpointValidator()
        {
          
        }
    }


}