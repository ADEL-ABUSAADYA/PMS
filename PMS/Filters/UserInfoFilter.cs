using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using PMS.Common;
using PMS.Common.Views;

public class UserInfoFilter : IActionFilter
{
    private readonly UserInfoProvider _userInfoProvider;

    public UserInfoFilter(UserInfoProvider userInfoProvider)
    {
        _userInfoProvider = userInfoProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity.IsAuthenticated)
        {
            var userId = Guid.TryParse(user.FindFirst("ID")?.Value, out var id) ? id : Guid.Empty;
            _userInfoProvider.UserInfo.ID = userId;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}