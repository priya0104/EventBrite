﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.ViewModels
{
    public class EventCatalogIndexViewModel
    {
        public PaginationInfo PaginationInfo { get; set; }
        public IEnumerable<SelectListItem> Catagories { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<EventItems> EventItems { get; set; }

        public int? CatagoryFilterApplied { get; set; }
        public int? TypeFilterApplied { get; set; }
    }
}
