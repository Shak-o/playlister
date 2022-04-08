using PlayLister.Services.Interfaces;

namespace PlayLister.Client.Middlewares
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthService service)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.Request.Cookies["timeout"]))
                {
                    var valid = DateTime.Parse(context.Request.Cookies["timeout"]);
                    if (valid < DateTime.Now)
                    {
                        var newToken = await service.RefreshToken(context.Request.Cookies["accessToken"],
                            context.Request.Cookies["refreshToken"]);
                        if (newToken != null)
                        {
                            context.Response.Cookies.Delete("timeout");
                            context.Response.Cookies.Delete("accessToken");
                            context.Response.Cookies.Append("timeout",
                                DateTime.Now.AddSeconds(newToken.Expires).ToString());
                            context.Response.Cookies.Append("accessToken", newToken.AccessToken);
                        }
                    }
                }
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
