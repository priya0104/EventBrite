using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public class CartService :ICartService
    {
        private readonly IConfiguration _config;
        private readonly string _remoteServiceBaseUrl;
        private IHttpClient _apiClient;
        private readonly ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;

        public CartService(IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IHttpClient httpClient,
            ILoggerFactory logger)
        {
            _config = config;
            _remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/v1/cart";
            _apiClient = httpClient;
            _logger = logger.CreateLogger<CartService>();
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task AddItemToCart(ApplicationUser user, CartItem product)
        {
            var cart = await GetCart(user);
            _logger.LogDebug("User Name: " + user.Id);
            if (cart == null)
            {
                cart = new Cart()
                {
                    BuyerId = user.Id,
                    Items = new List<CartItem>()
                };
            }

            var basketItem = cart.Items
                .Where(p => p.EventId == product.EventId)
                .FirstOrDefault();
            if (basketItem == null)
            {
                cart.Items.Add(product);
            }
            else
            {
                basketItem.Quantity += 1;
            }

            await UpdateCart(cart);
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            var token = await GetUserTokenAsync();
            _logger.LogDebug("service url: " + _remoteServiceBaseUrl);
            var updateBasketUrl = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update basket Url: " + updateBasketUrl);
            var response = await _apiClient.PostAsync(updateBasketUrl, cart, token);
            response.EnsureSuccessStatusCode();

            return cart;
        }

        public async Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var basket = await GetCart(user);

            //quantities is dictionary
            basket.Items.ForEach(x =>
            {
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });
            return basket;
        }

        public async Task<Cart> GetCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            _logger.LogInformation("we are in get basket and user id" + user.Id);
            _logger.LogInformation(_remoteServiceBaseUrl);

            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Id);
            _logger.LogInformation(getBasketUri);
            var dataString = await _apiClient.GetStringAsync(getBasketUri, token);
            _logger.LogInformation(dataString);

            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ??
                new Cart()
                {
                    BuyerId = user.Id
                };
            return response;
        }

        public async Task ClearCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var clearBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Id);
            _logger.LogDebug("Clean basket Uri: " + clearBasketUri);
            var response = await _apiClient.DeleteAsync(clearBasketUri);
            _logger.LogDebug("basket cleaned");
        }

        public Order MapCartToOrder(Cart cart)
        {
            var order = new Order();
            order.OrderTotal = 0;

            cart.Items.ForEach(x =>
            {
                order.OrderItems.Add(new OrderItem()
                {
                    ProductId = int.Parse(x.EventId),

                    PictureURI = x.PictureUrl,
                    ProductName = x.EventName,
                    Units = x.Quantity,
                    UnitPrice = x.UnitPrice
                });
                order.OrderTotal += (x.Quantity * x.UnitPrice);
            });

            return order;
        }


        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            return await context.GetTokenAsync("access_token");
        }
    }
}
