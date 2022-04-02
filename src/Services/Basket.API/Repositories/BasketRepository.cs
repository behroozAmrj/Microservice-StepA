using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redis)
    {
        this._redisCache = redis ?? throw new ArgumentNullException(nameof(redis));
    }
    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var basket = await _redisCache.GetStringAsync(userName);
        if (string.IsNullOrEmpty(basket))
            return null;

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }
    public async Task<ShoppingCart> UpdateBsket(ShoppingCart basket)
    {
        await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
        return await this.GetBasket(basket.UserName);
    }
    public async Task DeleteBasket(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }


}
