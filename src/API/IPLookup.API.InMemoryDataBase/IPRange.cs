using System;

namespace IPLookup.API.InMemoryDataBase
{
    public class IPRange : IByValueBinarySearchObject
    {
        private const int RANGE_ROW_SIZE = 12;

        public uint IpFrom { get; }
        public uint IpTo { get; }
        public uint LocationIndex { get; }

        private byte[] dataBase;

        public IPRange(byte[] dataBase, uint index)
        {
            var offset = new GeoBaseHeader(dataBase).OffsetRanges;
            var startIndex = (int)(offset + (RANGE_ROW_SIZE) * index);
            if (dataBase.Length < startIndex + RANGE_ROW_SIZE)
                throw new InvalidOperationException("Can't read Ip Range Row. Not enough data in DataBase.");
            this.dataBase = dataBase;

            var ipFromStartIndex = startIndex;
            IpFrom = BitConverter.ToUInt32(dataBase, ipFromStartIndex);

            var ipToStartIndex = startIndex + 4;
            IpTo = BitConverter.ToUInt32(dataBase, ipToStartIndex);

            var locationIndexStartIndex = ipToStartIndex + 4;
            LocationIndex = BitConverter.ToUInt32(dataBase, locationIndexStartIndex);
        }

        public bool ContainsValue(byte[] value)
        {
            var ip = BitConverter.ToUInt32(value, 0);
            return IpFrom <= ip && ip <= IpTo;
        }

        public bool Less(byte[] value)
        {
            var ip = BitConverter.ToUInt32(value, 0);
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