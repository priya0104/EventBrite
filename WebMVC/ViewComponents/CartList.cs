using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.ViewComponents
{
    public class CartList: ViewComponent 
    {
        private readonly ICartService _cartService;

        public CartList(ICartService cartService) => _cartService = cartService;

        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new Models.CartModels.Cart();
            try
            {
                vm = await _cartService.GetCart(user);

                return View(vm);
            }
            catch
            {
                ViewBag.IsBasketInoperative = true;
                TempData["BasketInoperativeMsg"] = "Basket service is not running, Please try again later";
            }
            return View(vm);
        }
    }
}
