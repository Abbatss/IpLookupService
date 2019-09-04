using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IPLookup.API.InMemoryDataBase
{
    public class GeoDataBaseClient : IInMemoryGeoDataBase
    {
        private static byte[] DataBase;
        private const int HEADER_ROW_SIZE = 60;
        private readonly string DataBasefilePath;
        private Lazy<GeoBaseHeader> header = new Lazy<GeoBaseHeader>(() => ReadHeader());

        public IRowObjectFactory Factory { get; }
        public GeoDataBaseClient(string dataBasefilePath) : this(dataBasefilePath, new GeoDataBaseRowObjectFactory())
        {
        }
        public GeoDataBaseClient(string dataBasefilePath, IRowObjectFactory factory)
        {
            if (string.IsNullOrWhiteSpace(dataBasefilePath))
            {
                throw new ArgumentException("dataBasefilePath is empty", nameof(dataBasefilePath));
            }

            DataBasefilePath = dataBasefilePath;
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        private static GeoBaseHeader ReadHeader()
        {
            if (DataBase == null)
            {
                throw new InvalidOperationException("Please initialize Db first");

            }
            return new GeoBaseHeader(DataBase);
        }

        public void Init()
        {
            DataBase = File.ReadAllBytes(/*"./DataBase/geobase.dat"*/DataBasefilePath);
            if (DataBase.Length < HEADER_ROW_SIZE)
            {
                throw new GeoDataBaseException("DataBase  file is corrupted");
            }
        }
        public GeoBaseHeader GetHeader()
        {
            return header.Value;
        }
        public Task<T> SearchFirstItemByValue<T>(string value)
             where T : class, IByValueBinarySearchObject
        {
            return Task.FromResult(DataBase.ValueBinarySearch<T>(header.Value.Records, value, Factory));
        }
        public Task<List<T>> GetItems<T>(int start, int count)
             where T : class, IByValueBinarySearchObject
        {
            var list = new List<T>();
            var end = start + count;
            if (start >= 0)
            {
                for (int i = start; i < end && i < header.Value.Records; i++)
                {
                    list.Add(Factory.CreateInstance<T>(DataBase, i));
                }
            }
            return Task.FromResult(list);
        }
        public Task<T> Get<T>(int index)
            where T : class
        {
            return Task.FromResult(Factory.CreateInstance<T>(DataBase, index));
        }

        public Task<T> Scan<T>(string value) where T : class, IByValueBinarySearchObject
        {
            for (int i = 0; i < header.Value.Records; i++)
            {
                var item = Factory.CreateInstance<T>(DataBase, i);
                if (item.ContainsValue(value))
                {
                    return Task.FromResult<T>(item);
                }
            }
            return Task.FromResult<T>(null);
        }
    }
}
