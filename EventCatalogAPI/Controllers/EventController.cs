using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogAPI.Data;
using EventCatalogAPI.Domain;
using EventCatalogAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventContext _context;
        private readonly IConfiguration _config;

        public EventController(EventContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageIndex = 0, [FromQuery]int pageSize = 4)
        {
            var itemsCount = await _context.EventItems.LongCountAsync();
            var items = await _context.EventItems
                .OrderBy(e => e.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel<EventItem>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Count = itemsCount,
                Data = items
            };

            return Ok(model);
        }

        private List<EventItem> ChangePictureUrl(List<EventItem> items)
        {
            items.ForEach(e => e.PictureURI = e.PictureURI
            .Replace("http://externalcatalogbaseurltobereplaced/", _config["ExternalCatalogBaseUrl"]));

            return items;
        }

        [HttpGet]
        [Route("[action]/type/{eventTypeId}/catagory/{eventCatagoryId}")]
        public async Task<IActionResult> Items(
            int? eventTypeId,
            int? eventCatagoryId,           
            [FromQuery] int pageIndex = 0,
            [FromQuery]int pageSize = 4)
        {
            var root = (IQueryable<EventItem>)_context.EventItems;
            if (eventTypeId.HasValue)
            {
                root = root.Where(e => e.EventTypeId == eventTypeId);
            }

            if (eventCatagoryId.HasValue)
            {
                root = root.Where(e => e.EventCatagoryId == eventCatagoryId);
            }
            var itemsCount = await root.LongCountAsync();
            var items = await root
                .OrderBy(e => e.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel<EventItem>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Count = itemsCount,
                Data = items
            };

            return Ok(model);
        }       

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> EventTypes()
        {
            var items = await _context.EventTypes.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route ("[action]")]
        public async Task<IActionResult> EventCatagories()
        {
            var items = await _context.EventCatagories.ToListAsync();
            return Ok(items);
        }
    }
}