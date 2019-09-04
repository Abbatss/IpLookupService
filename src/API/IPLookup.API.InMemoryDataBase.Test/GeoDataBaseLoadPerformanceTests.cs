using System;
using System.Diagnostics;
using IPLookup.API.InMemoryDataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPLookup.API.Host.Tests
{
    [TestClass]
    public class GeoDataBaseLoadPerformanceTests
    {
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        [TestMethod]
        [DataRow(100)]
        public void Init_Test(int runCount)
        {
            var currentTime = InitDBTime();
            long minTime = currentTime;
            long maxTime = currentTime;
            for (int i = 0; i < runCount - 1; i++)
            {
                currentTime = InitDBTime();
                if (minTime > currentTime)
                {
                    minTime = currentTime;
                }
                if (maxTime < currentTime)
                {
                    maxTime = currentTime;
                }
            }
            TestContext.WriteLine($"dbClient Init time min: {minTime}");
            TestContext.WriteLine($"dbClient Init time max: {maxTime}");
        }
        private long InitDBTime()
        {
            var dbClient = new GeoDataBaseClient("./DataBase/geobase.dat");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            dbClient.Init();
            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds <= 30, sw.ElapsedMilliseconds.ToString());
            return sw.ElapsedMilliseconds;
        }
    }
}
