using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IEventCatalogService _service;
        //public CatalogController(IEventCatalogService service)
        //{
        //    _service = service;
        //}
        
        //onstructor can also be written like this with only one statement
        public CatalogController(IEventCatalogService service) =>
            _service = service;
        
        //it is async because, we dont want UI to be blocked
        public async Task<IActionResult> Index(
            int? catagoryFilterApplied,
            int? typeFilterApplied,
            int? page)
        {
            var itemsOnPage = 10;
            //firing a service call
            var eventCatalog = 
                await _service.GetEventItemsAsync(page ?? 0,
                itemsOnPage, catagoryFilterApplied, typeFilterApplied);
            var vm = new EventCatalogIndexViewModel
            {
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsOnPage,
                    TotalItems = eventCatalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)eventCatalog.Count / itemsOnPage)
                },
                EventItems = eventCatalog.Data,
                Catagories = await _service.GetCatagoriesAsync(),
                Types = await _service.GetTypesAsync(),
                CatagoryFilterApplied = catagoryFilterApplied ?? 0,
                TypeFilterApplied = typeFilterApplied ?? 0
            };
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages-1) ? "is-disabled" : "";

            return View(vm);
        }
    }
}