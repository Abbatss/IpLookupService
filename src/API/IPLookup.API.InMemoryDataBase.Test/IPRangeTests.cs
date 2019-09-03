using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IPLookup.API.InMemoryDataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPLookup.API.Host.Tests
{
    [TestClass]
    public class IPRangeTests
    {
        [TestMethod]
        public void Constructor_Test()
        {
            var expectedIpfrom = 12u;
            var expectedIpTo = 120u;
            var expectedIndex = 11u;
            var actual = GetIpRange(expectedIpfrom, expectedIpTo, expectedIndex);
            Assert.AreEqual(expectedIpfrom, actual.IpFrom);
            Assert.AreEqual(expectedIpTo, actual.IpTo);
            Assert.AreEqual(expectedIndex, actual.LocationIndex);
        }

        private static IPRange GetIpRange(uint expectedIpfrom, uint expectedIpTo, uint expectedIndex)
        {
            var ipRangeByte = new byte[72];
            BitConverter.GetBytes(60u).CopyTo(ipRangeByte, 48);
            BitConverter.GetBytes(expectedIpfrom).CopyTo(ipRangeByte, 60);
            BitConverter.GetBytes(expectedIpTo).CopyTo(ipRangeByte, 64);
            BitConverter.GetBytes(expectedIndex).CopyTo(ipRangeByte, 68);
            return new IPRange(ipRangeByte, 0u);
        }

        [TestMethod]
        public void Constructor_WrongDbSize_Test()
        {
            var ipRangeByte = new byte[65];
            BitConverter.GetBytes(60u).CopyTo(ipRangeByte, 48);
            Assert.ThrowsException<InvalidOperationException>(() => new IPRange(ipRangeByte, 0));
        }
        [TestMethod]
        public void IpRange_Contains_Test()
        {
            var ipRangeByte = GetIpRange(100u, 200u, 0);
            var ipByteContains = BitConverter.GetBytes(150u);
            Assert.IsTrue(ipRangeByte.ContainsValue("150.0.0.0"));
        }
        [TestMethod]
        public void IpRange_NotContains_Test()
        {
            var ipRangeByte = GetIpRange(100u, 200u, 0);
            var ipByteContains = BitConverter.GetBytes(250u);
            Assert.IsFalse(ipRangeByte.ContainsValue("250.0.0.0"));
        }
        [TestMethod]
        public void IpRange_Less_Test()
        {
            var ipRangeByte = GetIpRange(100u, 200u, 0);
            var ipByteContains = BitConverter.GetBytes(50u);
            Assert.IsTrue(ipRangeByte.Less("50.0.0.0"));
        }
        [TestMethod]
        public void IpRange_NotLess_Test()
        {
            var ipRangeByte = GetIpRange(100u, 200u, 0);
            var ipByteContains = BitConverter.GetBytes(150u);
            Assert.IsFalse(ipRangeByte.Less("150.0.0.0"));
        }

    }
}
