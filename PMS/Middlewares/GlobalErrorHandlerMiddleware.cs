using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Helpers;

namespace PMS.Middlewares;

/*public class GlobalErrorHandlerMiddleware
{
    RequestDelegate _nextAction;
    ILogger<GlobalErrorHandlerMiddleware> _logger;
    public GlobalErrorHandlerMiddleware(RequestDelegate nextAction, 
        ILogger<GlobalErrorHandlerMiddleware> logger)
    {
        _nextAction = nextAction;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _nextAction(context);
        }
        catch (Exception ex)
        {
            File.WriteAllText(@"F:\\log.txt", $"error{ex.Message}");
            var response = EndpointResponse<bool>.Failure(ErrorCode.UnKnownError);
            context.Response.WriteAsJsonAsync(response);
        }
    }
}*/

public class GlobalErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalErrorHandlerMiddleware(
        ILogger<GlobalErrorHandlerMiddleware> logger,
        IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " Unhandled exception occurred");

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Cannot write error response. Headers already sent.");
                throw;
            }

            context.Response.Clear();

            // ⬇️ تحديد كود الخطأ
            var errorCode = MapExceptionToErrorCode(ex);
            var statusCode = MapErrorCodeToStatusCode(errorCode);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            // ⬇️ الرسالة: في Dev نرجّع stack، وإلا الوصف الجاهز
            var message = _env.IsDevelopment()
                ? ex.ToString()
                : errorCode.GetDescription(); // من الـ Enum Helper

            var response = EndpointResponse<bool>.Failure(errorCode, message);

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private ErrorCode MapExceptionToErrorCode(Exception ex)
    {
        return ex switch
        {
            UnauthorizedAccessException => ErrorCode.Unauthorized,
            ArgumentException => ErrorCode.ValidationError,
            KeyNotFoundException => ErrorCode.ResourceNotFound,
            _ => ErrorCode.UnKnownError
        };
    }

    private int MapErrorCodeToStatusCode(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.ValidationError => StatusCodes.Status400BadRequest,
            ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCode.ResourceNotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
