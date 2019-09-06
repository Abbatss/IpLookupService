using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IPLookup.API.InMemoryDataBase;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace IPLookup.API.InMemoryDataBase.Test
{
    [TestClass]
    public class GeoDataBaseClientTests
    {
        public TestContext TestContext
        {
            get;
            set;
        }
        private readonly GeoDataBaseClient _client;
        private readonly GeoBaseHeader _header;
        public GeoDataBaseClientTests()
        {
            var db = new GeoDataBaseClient("./DataBase/geobase.dat");
            db.Init();
            _client = db;
            _header = db.GetHeader();
        }
        [TestMethod]
        public void Constructor_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new GeoDataBaseClient(""));
            Assert.ThrowsException<ArgumentNullException>(() => new GeoDataBaseClient("12", null));
        }
        [TestMethod]
        public void GetHeader_Test()
        {
            var header = _client.GetHeader();
            Assert.AreEqual(1, header.Version);
            Assert.AreEqual(10800060u, header.OffsetCities);
            Assert.AreEqual(1200060u, header.OffsetLocations);
            Assert.AreEqual(60u, header.OffsetRanges);
            Assert.AreEqual(100000, header.Records);
            Assert.AreEqual(1487167858ul, header.Timestamp);
        }
        [TestMethod]
        public async Task GetIpRanges_Test()
        {
            var ipRanges = await _client.GetItems<IPRange>(0, _header.Records);
            Assert.AreEqual(_header.Records, ipRanges.Count);
            var ipRanges2 = await _client.GetItems<IPRange>(10, 5);
            CollectionAssert.AreEquivalent(ipRanges.Skip(10).Take(5).ToList(), ipRanges2);
        }
        [TestMethod]
        public async Task GetIpRanges_OutOfIndex_Test()
        {
            Assert.AreEqual(0, (await _client.GetItems<IPRange>(-1, _header.Records)).Count);
            Assert.AreEqual(_header.Records, (await _client.GetItems<IPRange>(0, _header.Records + 1)).Count);
        }
        [TestMethod]
        public async Task GetIpRange_Test()
        {
            var expectedIpRange = (await _client.GetItems<IPRange>(1, 1))[0];
            var ipToSearch = expectedIpRange.IpFrom + 1;
            var actualIpRange = await _client.SearchFirstItemByValue<IPRange>(UintToIpSrting(ipToSearch));

            Assert.AreEqual(expectedIpRange, actualIpRange);

            Assert.AreEqual(55473u, actualIpRange.IpFrom);
            Assert.AreEqual(1, actualIpRange.LocationIndex);
            Assert.AreEqual(151737u, actualIpRange.IpTo);
        }

        [TestMethod]
        public async Task GetIpRange_MaxIp_Test()
        {
            var expectedIpRange = (await _client.GetItems<IPRange>(_header.Records - 1, 1))[0];
            var ipToSearch = expectedIpRange.IpTo;
            var actualIpRange = await _client.SearchFirstItemByValue<IPRange>(UintToIpSrting(ipToSearch));
            Assert.AreEqual(expectedIpRange, actualIpRange);

            ipToSearch = expectedIpRange.IpFrom;
            actualIpRange = await _client.SearchFirstItemByValue<IPRange>(UintToIpSrting(ipToSearch));
            Assert.AreEqual(expectedIpRange, actualIpRange);
        }

        [TestMethod]
        public async Task GetIpRange_NotFount_Test()
        {
            var expectedIpRange = (await _client.GetItems<IPRange>(_header.Records - 1, 1))[0];
            var ipToSearch = expectedIpRange.IpTo + 1;
            var actualIpRange = await _client.SearchFirstItemByValue<IPRange>(UintToIpSrting(ipToSearch));
            Assert.IsNull(actualIpRange);
        }
        private static string UintToIpSrting(uint ip)
        {
            return ip.ToIpString();
        }

        [TestMethod]
        public async Task GetCitiesIndex_Test()
        {
            var citiesIndex = await _client.GetItems<CitiesIndex>(0, _header.Records);
            Assert.AreEqual(_header.Records, citiesIndex.Count);
            Assert.AreEqual(citiesIndex[0], await _client.Get<CitiesIndex>(0));
            Assert.AreEqual(citiesIndex[1], await _client.Get<CitiesIndex>(1));
            Assert.AreEqual(citiesIndex[_header.Records - 1], await _client.Get<CitiesIndex>(_header.Records - 1));
        }

        [TestMethod]
        public async Task GetCitiesIndex_CityName_Order_Test()
        {
            var data = File.ReadAllBytes("./DataBase/geobase.dat");
            var citiesIndex = await _client.GetItems<CitiesIndex>(0, _header.Records);

            CompareCitiesItems(data, citiesIndex);
        }

        private void CompareCitiesItems(byte[] data, System.Collections.Generic.List<CitiesIndex> citiesIndex)
        {
            var isNotSorter = false;
            CitiesIndex preCityIndex;
            CitiesIndex curCityIndex;
            for (int i = 1; i < citiesIndex.Count - 1; i++)
            {
                preCityIndex = citiesIndex[i - 1];
                curCityIndex = citiesIndex[i];
                isNotSorter = CompareCityNamesByBytes(data, preCityIndex, curCityIndex, i);
                if (isNotSorter)
                {
                    break;
                }
            }
            Assert.IsTrue(isNotSorter, "Cities Index is sorted sorted");

        }

        private bool CompareCityNamesByBytes(byte[] data, CitiesIndex preCityIndex, CitiesIndex curCityIndex, int i)
        {
            var isNotSorter = false;
            for (int j = 0; j < 20; j++)
            {
                if (data[preCityIndex.LocationInfoIndex + j] > data[curCityIndex.LocationInfoIndex + j])
                {
                    isNotSorter = true;
                    TestContext.WriteLine($"Item Id index = {i - 1} CityName = {preCityIndex.Location.City} ");
                    TestContext.WriteLine($"Item Id index = {i} CityName = {curCityIndex.Location.City} ");
                    break;
                }
            }
            return isNotSorter;
        }

        [TestMethod]
        public async Task ScanCitiesIndex_Test()
        {
            var cityName = "cit_Ijid";
            var res = await _client.Scan<CitiesIndex>(cityName);
            Assert.AreEqual(cityName, res[0].Location.City);
        }


        [TestMethod]
        public async Task ScanCitiesIndex_NotFound_Test()
        {
            var res = await _client.Scan<CitiesIndex>("42424242");

            Assert.AreEqual(0, res.Count);
        }
    }
}
