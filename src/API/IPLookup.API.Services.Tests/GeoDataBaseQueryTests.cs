using System;
using System.IO;
using System.Threading.Tasks;
using IPLookup.API.InMemoryDataBase;
using IPLookup.API.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IPLookup.API.Services.Tests
{
    [TestClass]
    public class GeoDataBaseQueryTests
    {
        private readonly byte[] DataBase;
        public GeoDataBaseQueryTests()
        {
            DataBase = File.ReadAllBytes("./DataBase/geobase.dat");
        }
        [TestMethod]
        public void Constructor_Test()
        {

            Assert.ThrowsException<ArgumentNullException>(() => new GeoDataBaseQuery(null));
        }

        [TestMethod]
        public async Task GetLocationByIp_Null_Test()
        {
            var clientMoq = new Moq.Mock<IInMemoryGeoDataBase>();
            var ipToSearch = "12";
            clientMoq.Setup(p => p.SearchFirstItemByValue<IPRange>(ipToSearch)).Returns(Task.FromResult<IPRange>(null));
            var query = new GeoDataBaseQuery(clientMoq.Object);
            var res = await query.GetLocationByIp(ipToSearch);
            Assert.IsNull(res);
            clientMoq.Verify(p => p.SearchFirstItemByValue<IPRange>(ipToSearch), Times.Once);
        }
        [TestMethod]
        public async Task GetLocationByIp_Test()
        {
            var clientMoq = new Moq.Mock<IInMemoryGeoDataBase>();
            var ipToSearch = "12";
            var ipRange = new IPRange(DataBase, 0);
            var location = new LocationInfo(DataBase, ipRange.LocationIndex);
            clientMoq.Setup(p => p.SearchFirstItemByValue<IPRange>(ipToSearch)).Returns(Task.FromResult(ipRange));
            clientMoq.Setup(p => p.Get<LocationInfo>(ipRange.LocationIndex)).Returns(Task.FromResult(location));
            var query = new GeoDataBaseQuery(clientMoq.Object);
            var res = await query.GetLocationByIp(ipToSearch);
            CompareLocationToLocationModel(location, res);
            clientMoq.Verify(p => p.SearchFirstItemByValue<IPRange>(ipToSearch), Times.Once);
            clientMoq.Verify(p => p.Get<LocationInfo>(ipRange.LocationIndex), Times.Once);
        }
        [TestMethod]
        public async Task GetLocationByCity_Test()
        {
            var clientMoq = new Moq.Mock<IInMemoryGeoDataBase>();
            var cityToSearch = "city";
            var citiesIndex = new CitiesIndex(DataBase, 0);
            var citiesIndex2 = new CitiesIndex(DataBase, 1);
            clientMoq.Setup(p => p.SearchFirstItemByValue<CitiesIndex>(cityToSearch)).Returns(Task.FromResult(citiesIndex));
            clientMoq.Setup(p => p.Get<CitiesIndex>((int)citiesIndex2.ItemIndex)).Returns(Task.FromResult(citiesIndex2));
            var query = new GeoDataBaseQuery(clientMoq.Object);
            var res = await query.GetLocationsByCity(cityToSearch);
            CompareLocationToLocationModel(citiesIndex.Location, res[0]);
        }

        private static void CompareLocationToLocationModel(LocationInfo location, LocationModel locationModel)
        {
            Assert.AreEqual(location.City, locationModel.City);
            Assert.AreEqual(location.Country, locationModel.Country);
            Assert.AreEqual(location.Latitude, locationModel.Latitude);
            Assert.AreEqual(location.Longitude, locationModel.Longitude);
            Assert.AreEqual(location.Organization, locationModel.Organization);
            Assert.AreEqual(location.Postal, locationModel.Postal);
            Assert.AreEqual(location.Region, locationModel.Region);
        }
    }
}
