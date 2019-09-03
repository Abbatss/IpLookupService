using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPLookup.API.Host.Controllers;
using IPLookup.API.Services;
using IPLookup.API.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace IPLookup.API.Host.Tests
{
    [TestClass]
    public class IpControllerTests
    {
        private Moq.Mock<IGeoDataBaseQuery> queryMoq = new Moq.Mock<IGeoDataBaseQuery>();
        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IpController(null));
        }

        [TestMethod]
        public async Task GetLocationTest()
        {
            var locationsList = new List<LocationModel>() { new LocationModel() { City = "1", Latitude = 1, Longitude = 1.2344f },
            new LocationModel() { City = "2", Latitude = 2, Longitude = 2.2344f }};
            var ip = "123";
            queryMoq.Setup(p => p.GetLocationsByIp(ip)).Returns(Task.FromResult(locationsList[0]));
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocation(ip);
            queryMoq.Verify(p => p.GetLocationsByIp(ip), Times.Once);
            Assert.AreEqual(locationsList[0], res.Content); ;

        }
    }
}
