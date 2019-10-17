using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class EventItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURI { get; set; }
        public decimal Price { get; set; }

        public int EventTypeId { get; set; }
        public string  EventType { get; set; }

        public int EventCatagoryId { get; set; }
        public string EventCatagory { get; set; }

    }
}
