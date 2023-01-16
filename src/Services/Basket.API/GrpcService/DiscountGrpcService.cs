using Discount.Grpc.Protos;

namespace Basket.API.GrpcService;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService , IHttpContextAccessor httpContextAccessor)
    {
        this._discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(DiscountProtoService));
        this._httpContextAccessor = httpContextAccessor;

        //string businessHeader = httpContextAccessor.HttpContext.Request.Headers.First(x => x.Key.Contains("BusinesLayed")).Value ?? string.Empty;
    }
    
    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest { ProductName = productName };
        return await _discountProtoService.GetDiscountAsync(discountRequest);
    }


}
