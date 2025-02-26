using Microsoft.AspNetCore.Mvc.Filters;
using PMS.Common;

namespace PMS.Filters;

public class CancellationTokenFilter : IActionFilter
{
    private readonly CancellationTokenProvider _cancellationTokenProvider;
    public CancellationTokenFilter(CancellationTokenProvider provider)
    {
        _cancellationTokenProvider = provider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _cancellationTokenProvider.CancellationToken = context.HttpContext.RequestAborted;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // throw new NotImplementedException();
    }
}