
namespace IOT_API.Extensions;

public static class HttpContextExtension
{
    public static int GetUserId(this HttpContext? httpContext)
    {
        if (httpContext == null) throw new Exception("httpContext is null");
        return httpContext.Items["user_id"] as int? ?? throw new Exception("Uid not found in HttpContext.Items");
    }

    public static int GetProjectId(this HttpContext? httpContext)
    {
        if (httpContext == null) throw new Exception("httpContext is null");
        return httpContext.Items["project_id"] as int? ?? throw new Exception("ProjectId not found in HttpContext.Items");
    }
}