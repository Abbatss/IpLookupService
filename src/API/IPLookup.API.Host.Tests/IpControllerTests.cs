using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly JsonSerializer _serializer = JsonSerializer.Create();
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
            queryMoq.Setup(p => p.GetLocationByIp(ip)).Returns(Task.FromResult(locationsList[0]));
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocation(ip);
            queryMoq.Verify(p => p.GetLocationByIp(ip), Times.Once);
            Assert.AreEqual(locationsList[0], res.Content); ;

        }

        [TestMethod]
        public async Task GetLocation_InvalidIpTest()
        {
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocation("123.12");
            Assert.IsNull(res.Content);
            res = await controller.GetLocation("aa.12.12.12");
            Assert.IsNull(res.Content);
            res = await controller.GetLocation("777.12.12.12");
            Assert.IsNull(res.Content);
        }



        [TestMethod]
        public async Task GetLocationsTest()
        {
            var locationsList = new List<LocationModel>() {
                new LocationModel() { City = "1", Latitude = 1, Longitude = 1.2344f },
                new LocationModel() { City = "2", Latitude = 2, Longitude = 2.2344f }};
            var city = "123";
            queryMoq.Setup(p => p.GetLocationsByCity(city)).Returns(Task.FromResult(locationsList));
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocations(city);
            queryMoq.Verify(p => p.GetLocationsByCity(city), Times.Once);

            Assert.AreEqual(Serialize(locationsList), Serialize(res.Content)); ;

        }
        public string Serialize<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }
}
