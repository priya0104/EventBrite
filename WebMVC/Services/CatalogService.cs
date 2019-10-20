using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            _baseUri = $"{config["CatalogUrl"]}/api/event/";
            _client = client;
        }
        public async Task<IEnumerable<SelectListItem>> GetCatagoriesAsync()
        {
            var catagoryUri = ApiPaths.EventCatalog.GetAllCatagories(_baseUri);
            var dataString = await _client.GetStringAsync(catagoryUri);
            //adding a list wich has "All" to a list of catagories
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text = "All",
                    Selected = true
                }
            };
            //it converts the string into array
            var catagories = JArray.Parse(dataString);
            foreach(var catagory in catagories)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = catagory.Value<string>("id"),
                        Text = catagory.Value<string>("catagoryName")
                    }
                 );
            }
            return items;
        }

        //pull the ApiPaths and fire the calls to HttpClient to get hte data
        public async Task<Catalog> GetEventItemsAsync(int page, int size, int? catagory, int? type)
        {
            var eventItemsUri = ApiPaths.EventCatalog.GetAllEventItems(_baseUri,
                page, size, catagory, type);

            var dataString = await _client.GetStringAsync(eventItemsUri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);
            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typeUri = ApiPaths.EventCatalog.GetAllTypes(_baseUri);
            var dataString = await _client.GetStringAsync(typeUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var types = JArray.Parse(dataString);
            foreach(var type in types)
            {
                items.Add(new SelectListItem {
                    Value = type.Value<string>("id"),
                    Text = type.Value<string>("typeName")
                });
            }
            return items;
        }
    }
}
