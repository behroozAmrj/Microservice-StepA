using Basket.API.Entities;
using Basket.API.GrpcService;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly ILogger<BasketController> logger;

        public BasketController(IBasketRepository basketRepository 
                                ,DiscountGrpcService discountGrpcService 
                                ,ILogger<BasketController> logger)
        {
            this._basketRepository = basketRepository;
            this._discountGrpcService = discountGrpcService;
            this.logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart) , (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userNamme)
        { 
            var baseket = await _basketRepository.GetBasket(userNamme);
            return Ok(baseket ?? new ShoppingCart(userNamme));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            basket.Items.ForEach( async item =>
            {
                var coupon =  await _discountGrpcService.GetDiscount(item.ProductName);
                if (coupon != null)
                    item.Price -= coupon.Amount;
                else
                    logger.LogError($"{item.ProductName} NOT found!");

            });
            return Ok(await _basketRepository.UpdateBsket(basket));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket([FromBody] ShoppingCart basket)
        {
            await _basketRepository.DeleteBasket(basket.UserName);
            return Ok();
        }

        
    }
}
