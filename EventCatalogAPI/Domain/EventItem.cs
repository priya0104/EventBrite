using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Domain
{
    public class EventItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURI { get; set; }
        public decimal Price { get; set; }

        public int EventTypeId { get; set; }
        //letting know this table is related to other tables
        public virtual EventType EventType { get; set; }

        public int EventCatagoryId { get; set; }
        public virtual EventCatagory EventCatagory { get; set; }

    }
}
