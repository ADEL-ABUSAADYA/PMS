using PMS.Common.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using PMS.Features.Common.Users.Queries;


namespace PMS.Filters
{
    public class CustomizeAuthorizeAttribute : ActionFilterAttribute
    {
        
        Feature _feature;
        IMediator _mediator;

        public CustomizeAuthorizeAttribute(Feature feature, IMediator mediator)
        {
            _feature = feature;
            _mediator = mediator;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var claims = context.HttpContext.User;
             
            var userID = claims.FindFirst("ID");

            if (userID is null || string.IsNullOrEmpty(userID.Value))
            {
                throw new UnauthorizedAccessException();
            }

            var user = Guid.Parse(userID.Value);


            var hasAccess = await _mediator.Send(new HasAccessQuery(user, _feature));

            if (!hasAccess)
            {
                throw new UnauthorizedAccessException();
            }

            base.OnActionExecuting(context);
        }
    }
}
