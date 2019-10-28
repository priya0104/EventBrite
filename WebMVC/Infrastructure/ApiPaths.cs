using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    public class ApiPaths
    {
        //class to get possible ApiPaths for EventCatalogAPI microservice
        public static class EventCatalog
        {
            //to get all Event Types
            //baseUri can be changed and it will be provided by statup.cs 
            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}eventtypes";
            }

            public static string GetAllCatagories(string baseUri)
            {
                return $"{baseUri}eventcatagories";
            }

            //this is to get all items based on catagory/type and page number and number of rows per page
            public static string GetAllEventItems(string baseUri,int page,int take, int? catagory, int? type)
            {
                var filterQs = string.Empty;

                if (catagory.HasValue || type.HasValue)
                {
                    //if catagory has value, get that value
                    var catagoriesQs = (catagory.HasValue) ? catagory.Value.ToString() : "null";
                    var typesQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs = $"/type/{typesQs}/catagory/{catagoriesQs}";
                }

                //pageindex and page size are QueryParams
                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri,string basketId)
            {
                return $"{baseUri}/{basketId}";
            }

            public static string UpdateBasket(string baseUri)
            {
                return baseUri;
            }

            public static string CleanBasket(string baseUri,string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }
    }
}
