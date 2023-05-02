using System.Net.Http.Headers;
using System.Text;

public class BasicAuthMiddleware : IMiddleware
{
    private const string Realm = "My Realm";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{Realm}\"");
            context.Response.StatusCode = 401;
            return;
        }

        var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
        var split = credentials.Split(':', 2);
        var username = split[0];
        var password = split[1];

        if (username != "1" || password != "simcorp")
        {
            context.Response.StatusCode = 401;
            return;
        }

        await next(context);
    }
}