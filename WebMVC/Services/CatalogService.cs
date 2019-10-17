using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebMVC.Infrastructure;
using WebMVC.Models;

namespace WebMVC.Services
{
    //this should call the ApiPaths, Asking for URL and making the HTTP calls and then get the data back
    public class CatalogService : IEventCatalogService
    {
        private readonly IHttpClient _client;
        private readonly string _baseUri;

        public CatalogService(IConfiguration config,
            IHttpClient client)
        {
            _baseUri = $"{config["EventCatalogUrl"]}/api/catalog/";
            _client = client;
        }
        public Task<IEnumerable<SelectListItem>> GetCatagoriesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Catalog> GetEventItemsAsync(int page, int size, int? catagory, int? type)
        {
            var eventItemsUri = ApiPaths.EventCatalog.GetAllEventItems(_baseUri,
                page, size, catagory, type);

            var dataString = await _client.GetStringAsync(eventItemsUri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);
            return response;
        }

        public Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
