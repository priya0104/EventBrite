﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IEventCatalogService
    {
        Task<Catalog>GetEventItemsAsync(int page, int size, int? catagory, int? type);
        Task<IEnumerable<SelectListItem>> GetCatagoriesAsync();
        Task<IEnumerable<SelectListItem>> GetTypesAsync();
    }
}
