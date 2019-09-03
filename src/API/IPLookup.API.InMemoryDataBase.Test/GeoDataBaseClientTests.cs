﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IPLookup.API.InMemoryDataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPLookup.API.Host.Tests
{
    [TestClass]
    public class GeoDataBaseClientTests
    {
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
            Assert.AreEqual(0, (await _client.GetItems<IPRange>(0, _header.Records + 1)).Count);
        }
        [TestMethod]
        public async Task GetIpRange_Test()
        {
            var expectedIpRange = (await _client.GetItems<IPRange>(1, 1))[0];
            var ipToSearch = expectedIpRange.IpFrom + 1;
            var actualIpRange = await _client.SearchByValue<IPRange>(UintToIpSrting(ipToSearch));

            Assert.AreEqual(expectedIpRange, actualIpRange);

            Assert.AreEqual(55473u, actualIpRange.IpFrom);
            Assert.AreEqual(1u, actualIpRange.LocationIndex);
            Assert.AreEqual(151737u, actualIpRange.IpTo);
        }

        [TestMethod]
        public async Task GetIpRange_MaxIp_Test()
        {
            var expectedIpRange = (await _client.GetItems<IPRange>(_header.Records - 1, 1))[0];
            var ipToSearch = expectedIpRange.IpTo;
            var actualIpRange = await _client.SearchByValue<IPRange>(UintToIpSrting(ipToSearch));
            Assert.AreEqual(expectedIpRange, actualIpRange);

            ipToSearch = expectedIpRange.IpFrom;
            actualIpRange = await _client.SearchByValue<IPRange>(UintToIpSrting(ipToSearch));
            Assert.AreEqual(expectedIpRange, actualIpRange);
        }

        [TestMethod]
        public async Task GetIpRange_NotFount_Test()
        {
            var expectedIpRange = (await _client.GetItems<IPRange>(_header.Records - 1, 1))[0];
            var ipToSearch = expectedIpRange.IpTo + 1;
            var actualIpRange = await _client.SearchByValue<IPRange>(UintToIpSrting(ipToSearch));
            Assert.IsNull(actualIpRange);
        }
        private static string UintToIpSrting(uint ip)
        {
           var res = BitConverter.GetBytes(ip);
            
            var ipstring = $"{res[0].ToString()}.{res[1].ToString()}.{res[2].ToString()}.{res[3].ToString()}";
            return ipstring;
               
        }
    }
}
