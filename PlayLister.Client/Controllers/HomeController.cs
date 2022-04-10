﻿using Microsoft.AspNetCore.Mvc;
using PlayLister.Client.Models;
using System.Diagnostics;
using PlayLister.Services.Interfaces;
using PlayLister.Services.Models;
using PlayLister.Services.Models.ServiceModels;

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
        public async Task<IActionResult> ConvertList(string playlistLink)
        {
            var keyWord = "list=";
            int index = playlistLink.IndexOf(keyWord);
            string id = playlistLink.Substring(index + keyWord.Length);

            var list = await _converter.GetPlaylistItems(id);

            ViewBag.Pages = (list.TotalResults / list.ResultsPerPage);

            return View(list);
        }

        [HttpGet("listPage")]
        public async Task<IActionResult> Page(string id, int page)
        {
            PlaylistServiceModel data = await _converter.GetPlaylistItems(id, page);

            ViewBag.Pages = (data.TotalResults / data.ResultsPerPage);
        
            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}