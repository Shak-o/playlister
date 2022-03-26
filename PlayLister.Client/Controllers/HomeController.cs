using Microsoft.AspNetCore.Mvc;
using PlayLister.Client.Models;
using System.Diagnostics;
using PlayLister.Services.Interfaces;
using PlayLister.Services.Models;

namespace PlayLister.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlaylistConverter _converter;

        public HomeController(ILogger<HomeController> logger, IPlaylistConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public async Task<IActionResult> Index()
        {
            //var list = await _converter.GetYoutubePlaylists("UC7E2aOwJF2-7xXQJ5xImTEQ");
            return View();
        }

        [HttpGet("list")]
        public async Task<IActionResult> ConvertList(string channelLink, string playlistName)
        {
            int index = channelLink.IndexOf("channel/");
            string id = channelLink.Substring(index + 8);

            var list = await _converter.GetPlaylistItems(id, playlistName);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}