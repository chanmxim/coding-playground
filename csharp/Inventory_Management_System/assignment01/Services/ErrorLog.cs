using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace assignment01.Models;

public class ErrorLog
{
    private readonly StackTrace _stackTrace;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ErrorLog> _logger;


    public ErrorLog(StackTrace stackTrace, IHttpContextAccessor httpContextAccessor, ILogger<ErrorLog> logger)
    {
        _stackTrace = stackTrace;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public void LogError(Exception exception)
    {
        var stackTrace = _stackTrace.GetFrame(1).GetMethod();
        _logger.LogError(
            $"{DateTime.Now} - User ID: {_httpContextAccessor.HttpContext.User.Identity.GetUserId()} " +
            $"{stackTrace.DeclaringType.Name} -  {stackTrace.Name} - {exception.Message}");
    }
    
}