using System;
using System.Collections.Generic;
using IOFile = System.IO.File;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        //access the folders in wwwroot via injection
        public PicController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            //accessing wwwroot folder
            var webRoot = _env.WebRootPath;
            //path to png on the server
            var path = Path.Combine($"{webRoot}/Pics/", $"event{id}.png");
            var buffer = IOFile.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
    }
}