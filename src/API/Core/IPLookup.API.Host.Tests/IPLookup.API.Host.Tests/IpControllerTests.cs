using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.IPLookup.API.Host.Controllers;
using IPLookup.API.Services;
using IPLookup.API.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IPLookup.API.Host.Tests
{
    public class IpControllerTests
    {
        private readonly JsonSerializer _serializer = JsonSerializer.Create();
        private Moq.Mock<IGeoDataBaseQuery> queryMoq = new Moq.Mock<IGeoDataBaseQuery>();
        [Test]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>(() => new IpController(null));
        }

        [Test]
        public async Task GetLocationTest()
        {
            var locationsList = new List<LocationModel>() { new LocationModel() { City = "1", Latitude = 1, Longitude = 1.2344f },
            new LocationModel() { City = "2", Latitude = 2, Longitude = 2.2344f }};
            var ip = "123.123.123.123";
            queryMoq.Setup(p => p.GetLocationByIp(ip)).Returns(Task.FromResult(locationsList[0]));
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocationByIp(ip);
            queryMoq.Verify(p => p.GetLocationByIp(ip), Times.Once);
            Assert.AreEqual(locationsList[0], (res as OkObjectResult).Value); ;

        }

        [Test]
        public async Task GetLocation_InvalidIpTest()
        {
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocationByIp("123.12");
            res = await controller.GetLocationByIp("aa.12.12.12");
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsNull((res as OkObjectResult).Value);
            res = await controller.GetLocationByIp("777.12.12.12");
            Assert.IsNull((res as OkObjectResult).Value);
        }

        [Test]
        public async Task GetLocationsTest()
        {
            var locationsList = new List<LocationModel>() {
                new LocationModel() { City = "1", Latitude = 1, Longitude = 1.2344f },
                new LocationModel() { City = "2", Latitude = 2, Longitude = 2.2344f }};
            var city = "123";
            queryMoq.Setup(p => p.GetLocationsByCity(city)).Returns(Task.FromResult(locationsList));
            var controller = new IpController(queryMoq.Object);
            var res = await controller.GetLocationsByCity(city);
            queryMoq.Verify(p => p.GetLocationsByCity(city), Times.Once);

            Assert.AreEqual(Serialize(locationsList), Serialize((res as OkObjectResult).Value)); ;

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
