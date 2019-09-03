using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IPLookup.API.InMemoryDataBase
{
    public class GeoDataBaseClient: IInMemoryGeoDataBase
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
        public Task<T> Get<T>(byte[] ip)
             where T : class, IByValueBinarySearchObject
        {
            if (ip.Length != 4)
            {
                throw new ArgumentException("ip is not valid");
            }
            return Task.FromResult(DataBase.ValueBinarySearch<T>((uint)header.Value.Records, ip, Factory));
        }
        public Task<List<T>> GetItems<T>(int start, int count)
             where T : class, IByValueBinarySearchObject
        {
            var list = new List<T>();
            var end = start + count;
            if (start >= 0 && end <= header.Value.Records)
            {
                for (int i = start; i < end; i++)
                {
                    list.Add(Factory.CreateInstance<T>(DataBase, (uint)i));
                }
            }
            return Task.FromResult(list);
        }

    }
}
