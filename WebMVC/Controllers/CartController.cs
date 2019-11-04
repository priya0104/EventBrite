using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IEventCatalogService _catalogService;
        private readonly IIdentityService<ApplicationUser> _identityService;

        public CartController(IIdentityService<ApplicationUser> identityService,
            IEventCatalogService catalogService,
            ICartService cartService)
        {
            _identityService = identityService;
            _cartService = cartService;
            _catalogService = catalogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(
            Dictionary<string, int> quantities,
            string action)
        {
            if (action == "[ Checkout ]")
            {
                return RedirectToAction("Create", "Order");
            }

            try
            {
                var user = _identityService.Get(HttpContext.User);
                var basket = await _cartService.SetQuantities(user, quantities);
                var vm = await _cartService.UpdateCart(basket);
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }

            return View();
        }

        public async Task<IActionResult> AddToCart(EventItems productDetails)
        {
            try
            {
                if(productDetails.Id != null)
                {
                    var user = _identityService.Get(HttpContext.User);
                    var product = new CartItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        EventName = productDetails.Name,
                        PictureUrl = productDetails.PictureURI,
                        UnitPrice = productDetails.Price,
                        EventId = productDetails.Id
                    };
                    await _cartService.AddItemToCart(user, product);
                }
            }
            catch(BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }

            return RedirectToAction("Index", "Catalog");
        }

        private void HandleBrokenCircuitException()
        {
            TempData["BasketInoperativeMsg"] = "Cart Service is inoperative,please try later on.";
        }
    }
}