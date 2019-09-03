using System;
using System.Net;

namespace IPLookup.API.InMemoryDataBase
{
    public class IPRange : IByValueBinarySearchObject
    {
        private const int RANGE_ROW_SIZE = 12;

        public uint IpFrom { get; }
        public int IpToStartIndex { get; private set; }
        public uint IpTo { get; }
        public uint LocationIndex { get; }

        private byte[] dataBase;

        public int IpFromStartIndex { get; private set; }

        public IPRange(byte[] dataBase, uint index)
        {
            var offset = new GeoBaseHeader(dataBase).OffsetRanges;
            var startIndex = (int)(offset + (RANGE_ROW_SIZE) * index);
            if (dataBase.Length < startIndex + RANGE_ROW_SIZE)
                throw new InvalidOperationException("Can't read Ip Range Row. Not enough data in DataBase.");
            this.dataBase = dataBase;

            IpFromStartIndex = startIndex;
            IpFrom = BitConverter.ToUInt32(dataBase, IpFromStartIndex);

            IpToStartIndex = startIndex + 4;
            IpTo = BitConverter.ToUInt32(dataBase, IpToStartIndex);

            var locationIndexStartIndex = IpToStartIndex + 4;
            LocationIndex = BitConverter.ToUInt32(dataBase, locationIndexStartIndex);
        }

        public bool ContainsValue(string value)
        {
            var ip = ConvertToUint(value);
            return IpFrom <= ip && ip <= IpTo;
        }

        public static uint ConvertToUint(string value)
        {
            var addr = IPAddress.Parse(value).GetAddressBytes();
            
            var ipBytes = value.Split('.');
            if (ipBytes.Length != 4)
            {
                throw new InvalidOperationException("value has wrong format when convert to ip");
            }
            var ip = new byte[0];
            if (byte.TryParse(ipBytes[0], out var firstByte)
                && byte.TryParse(ipBytes[1], out var secondByte)
                && byte.TryParse(ipBytes[2], out var thirdByte)
                && byte.TryParse(ipBytes[3], out var fouthByte)
                )
            {
                return BitConverter.ToUInt32(new byte[] { firstByte, secondByte, thirdByte, fouthByte }, 0);
            }
            throw new InvalidOperationException("value has wrong format when convert to ip");
        }

        public bool Less(string value)
        {
            var ip = ConvertToUint(value);
            return IpFrom > ip;
        }

        public override bool Equals(object obj)
        {
            return obj is IPRange range &&
                   IpFrom == range.IpFrom &&
                   IpTo == range.IpTo &&
                   LocationIndex == range.LocationIndex;
        }

        public override int GetHashCode()
        {
            var hashCode = -763417269;
            hashCode = hashCode * -1521134295 + IpFrom.GetHashCode();
            hashCode = hashCode * -1521134295 + IpTo.GetHashCode();
            hashCode = hashCode * -1521134295 + LocationIndex.GetHashCode();
            return hashCode;
        }
    }
}