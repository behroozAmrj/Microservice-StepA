using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class TestService : TestProtoService.TestProtoServiceBase
    {
        public TestService() { }

        public override Task<ReturnModel> GetTestResult(CreateTestRequest request, ServerCallContext context)
        {
            var retVal = new ReturnModel() { DateTime = DateTime.Now.ToString()  , Name = request.Id > 1 ? "Is greater" : "Not reater"};
            return Task.FromResult(retVal);

        }
    }
}
