public class TimeOutMiddleware : IMiddleware
{
    private static readonly object TokenKey = new();

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, context.RequestAborted);

        context.Items[TokenKey] = linkedCts.Token;

        Task requestTask = next(context);
        Task delayTask = Task.Delay(TimeSpan.FromSeconds(10), linkedCts.Token);

        if (delayTask == await Task.WhenAny(requestTask, delayTask))
        {
            timeoutCts.Cancel();
            context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
            await context.Response.WriteAsync("Request timed out.");
        }
    }
}