using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IPLookup.API.InMemoryDataBase
{
    public class GeoDataBaseClient
    {
        private static byte[] DataBase;
        private const int HEADER_ROW_SIZE = 60;
        private readonly string DataBasefilePath;
        private Lazy<GeoBaseHeader> header = new Lazy<GeoBaseHeader>(() => ReadHeader());

        public GeoDataBaseClient(string dataBasefilePath)
        {
            if (string.IsNullOrWhiteSpace(dataBasefilePath))
            {
                throw new ArgumentException("dataBasefilePath is empty", nameof(dataBasefilePath));
            }

            DataBasefilePath = dataBasefilePath;
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
        public Task<IPRange> GetIpRange(Byte[] ip)
        {
            if (ip.Length != 4)
            {
                throw new ArgumentException("ip is not valid");
            }
            return Task.FromResult(DataBase.IpBinarySearch(header.Value.OffsetRanges, header.Value.Records, ip));
        }
        public Task<List<IPRange>> GetIpRanges(int start, int count)
        {
            var list = new List<IPRange>();
            var end = start + count;
            if (start >= 0 && end <= header.Value.Records)
            {
                for (int i = start; i < end; i++)
                {
                    list.Add(new IPRange(DataBase, header.Value.OffsetRanges, i));
                }
            }
            return Task.FromResult(list);
        }

    }
}
