using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class RedisCartRepository: ICartRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private IDatabase _database;

        public RedisCartRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }

        public async Task<bool> DeleteCartAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<Cart> GetCartAsync(string cartId)
        {
            //jason data from the redis DB
            var data = await _database.StringGetAsync(cartId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Cart>(data);

        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            //keys is the basket ID
            var data = server.Keys();
            return data?.Select(k => k.ToString());
        }

        //to get the cache closer to me 
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task<Cart> UpdateCartAsync(Cart basket)
        {
            //locate the cart by BuyerId. update the cache of the particular
            // buyer with the basket data
            var created = await _database.StringSetAsync(basket.BuyerId,
               JsonConvert.SerializeObject(basket));
            if (!created)
            {
                //_logger.LogInformation("Problem occur persisting")
                return null;
            }
            //_logger.LogInformation("Basket item persisted successful");

            return await GetCartAsync(basket.BuyerId);
        }
    }
}
