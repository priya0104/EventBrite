using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    //this is equivalant to PaginatedViewItemsModel class in Microservice
    public class Catalog
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public long Count { get; set; }

        public List<EventItems> Data { get; set; }
    }
}
