using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcService;
using Basket.API.Repositories;
using EventBus.Message.Evnets;
using MassTransit;
using MassTransit.Internals;
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
        private readonly IMapper _mapper;
         private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository basketRepository
                                , DiscountGrpcService discountGrpcService
                                , IMapper mapper
                                , IPublishEndpoint publishEndpoint
                                , ILogger<BasketController> logger)
        {
            this._basketRepository = basketRepository;
            this._discountGrpcService = discountGrpcService;
            this._mapper = mapper;
            this._publishEndpoint = publishEndpoint;
            this._logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userNamme)
        {
            var baseket = await _basketRepository.GetBasket(userNamme);
            return Ok(baseket ?? new ShoppingCart(userNamme));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            basket.Items.ForEach(async item =>
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                if (coupon != null)
                    item.Price -= coupon.Amount;
                else
                    _logger.LogError($"{item.ProductName} NOT found!");

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

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //get existing basket with total price
            await _publishEndpoint.Publish(new BasketCheckoutEvent() { UserName = "Beh" });
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
                return BadRequest();

            // cerate basket checkoutEvent -- Set TotalPrice on basketchekcout event message

            var eventMessge = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessge.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessge);

            //remove the basket
            //await _basketRepository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}
