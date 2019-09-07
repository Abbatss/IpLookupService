using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

namespace IPLookup.API.Host.Lambda.Tests
{
    public class IpControllerTests
    {
        [TestCase("IpController - Get.json")]
        public async Task TestGet(string requestJsonFileName)
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText($"./SampleRequests/{requestJsonFileName}");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);
            Assert.AreEqual(200, response.StatusCode);
        }
    }
}