using AutoMapper;
using Basket.API.Entities;
using EventBus.Message.Evnets;

namespace Basket.API.Mapper;

public class BasketProfile : Profile
{
	public BasketProfile()
	{
		CreateMap<BasketCheckout , BasketCheckoutEvent>().ReverseMap();
	}
}
