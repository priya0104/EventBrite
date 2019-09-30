using EventCatalogAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Data
{
    public class EventSeed
    {
        public static void Seed(EventContext context)
        {
            //to migrate data automatically
            context.Database.Migrate();
            //seeding: skip adding data if there is data already
            if (!context.EventCatagories.Any())
            {
                //inserting an array of fields
                context.EventCatagories.AddRange(GetPreConfiguredEventCatagories());
                //once added, save changes to the table
                context.SaveChanges();
            }
            if (!context.EventTypes.Any())
            {
                context.EventTypes.AddRange(GetPreConfiguredEventTypes());

                context.SaveChanges();
            }

            if (!context.EventItems.Any())
            {
                context.EventItems.AddRange(GetPreConfiguredEventItems());
                context.SaveChanges();
            }
        }

        private static IEnumerable<EventItem> GetPreConfiguredEventItems()
        {
            return new List<EventItem>()
            {
                new EventItem() { EventCatagoryId=1,EventTypeId=2, Description = "will make you world champions", Name = "Homebuyers Workshop - Kirkland Library", Price= 88.50M, PictureURI = "http://externalcatalogbaseurltobereplaced/api/pic/2" },
                new EventItem() { EventCatagoryId=2,EventTypeId=1, Description = "will make you world champions", Name = "Kirkland Business Round Table", Price= 15.50M, PictureURI = "http://externalcatalogbaseurltobereplaced/api/pic/2" },
                new EventItem() { EventCatagoryId=3,EventTypeId=2, Description = "will make you world champions", Name = "Startup 425 Foundations", Price= 17.50M, PictureURI = "http://externalcatalogbaseurltobereplaced/api/pic/2" },
                new EventItem() { EventCatagoryId=2,EventTypeId=3, Description = "will make you world champions", Name = "NAIOMT C-725A Advanced Spinal Manipulation Part A", Price= 21.50M, PictureURI = "http://externalcatalogbaseurltobereplaced/api/pic/2" },
                new EventItem() { EventCatagoryId=2,EventTypeId=1, Description = "will make you world champions", Name = "Yoga for Bigginers", Price= 30.50M, PictureURI = "http://externalcatalogbaseurltobereplaced/api/pic/2" },
                new EventItem() { EventCatagoryId=3,EventTypeId=1, Description = "will make you world champions", Name = "Women's Self Defense Semina", Price= 150.00M, PictureURI = "http://externalcatalogbaseurltobereplaced/api/pic/2" },

            };
        }

        private static IEnumerable<EventType> GetPreConfiguredEventTypes()
        {
            return new List<EventType>()
            {
                new EventType() {TypeName = "Class"},
                new EventType() {TypeName = "Tour"},
                new EventType() {TypeName = "Rally"}
            };
        }

        private static IEnumerable<EventCatagory> GetPreConfiguredEventCatagories()
        {
            //add list of type eventCatalogs to the table
            return new List<EventCatagory>()
            {
                new EventCatagory() {CatagoryName = "Business"},
                new EventCatagory() {CatagoryName = "Music"},
                new EventCatagory() {CatagoryName = "Health"}
            };
        }
    }
}
