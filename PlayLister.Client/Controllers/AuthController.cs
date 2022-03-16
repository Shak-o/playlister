using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PlayLister.Client.Extensions;
using PlayLister.Services.Interfaces;

namespace PlayLister.Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return Redirect(_service.GetUri());
        }

        [HttpGet("/auth/id")]
        public IActionResult GetCode(string code)
        {
            return Redirect($"/auth/saveToken?code={code}");
        }

        [HttpGet("/auth/saveToken")]
        public async Task<IActionResult> SaveToken(string code)
        {
            var appData = await _service.RequestToken(code);
            HttpContext.Response.Cookies.Append("accessToken", appData.AccessToken);
            HttpContext.Response.Cookies.Append("refreshToken", appData.RefreshToken);
            HttpContext.Response.Cookies.Append("timeout", DateTime.Now.AddSeconds(appData.ExpiresIn).ToString());
            return Redirect("/");
        }
    }
}
