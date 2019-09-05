using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IPLookup.API.InMemoryDataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPLookup.API.Host.Tests
{
    [TestClass]
    public class CitiesIndexTests
    {
        [TestMethod]
        public void Constructor_Test()
        {
            var locationInfoIndex = 64u;
            var actual = GetCitiesIndex(locationInfoIndex, "city");
            Assert.AreEqual(locationInfoIndex + LocationInfo.CITY_NAME_OFFSET, actual.LocationInfoIndex);
            Assert.IsNotNull(actual.Location);
        }

        private static CitiesIndex GetCitiesIndex(uint locationInfoIndex, string city)
        {
            var db = new byte[160];
            BitConverter.GetBytes(60u).CopyTo(db, 52);
            BitConverter.GetBytes(locationInfoIndex + LocationInfo.CITY_NAME_OFFSET).CopyTo(db, 60);
            var cityBytes = System.Text.UTF8Encoding.UTF8.GetBytes(city);
            cityBytes.CopyTo(db, 60 + 4 + 32);
            return new CitiesIndex(db, 0);
        }

        [TestMethod]
        public void Constructor_WrongDbSize_Test()
        {
            var db = new byte[61];
            BitConverter.GetBytes(60u).CopyTo(db, 52);
            Assert.ThrowsException<InvalidOperationException>(() => new CitiesIndex(db, 0));
        }
        [TestMethod]
        public void CitiesIndex_Contains_Test()
        {
            var cityIndex = GetCitiesIndex(64u, "cit_A A");
            Assert.IsTrue(cityIndex.ContainsValue("cit_A A"));
        }
        [TestMethod]
        public void CitiesIndex_NotContains_Test()
        {
            var cityIndex = GetCitiesIndex(64u, "cit_A A");
            Assert.IsFalse(cityIndex.ContainsValue("cit_AB"));
        }
        [TestMethod]
        public void CitiesIndex_LessThan_Test()
        {
            var cityIndex = GetCitiesIndex(64u, "cit_AA");
            Assert.IsTrue(cityIndex.LessThan("cit_AB"));
        }
        [TestMethod]
        public void CitiesIndex_NotLess_Test()
        {
            var cityIndex = GetCitiesIndex(64u, "cit_AB");
            Assert.IsFalse(cityIndex.LessThan("cit_AA"));
        }
        [TestMethod]
        public void CitiesIndex_Less_Null_Test()
        {
            var cityIndex = GetCitiesIndex(64u, "cit_AA");
            Assert.IsFalse(cityIndex.LessThan(null));
        }

    }
}
