using System;

namespace IPLookup.API.InMemoryDataBase
{
    public class GeoBaseHeader
    {
        private const int NAME_LENGTH = 32;
        public int Version { get; }
        public string Name { get; }
        public ulong Timestamp { get; }
        public int Records { get; }
        public uint OffsetRanges { get; }
        public uint OffsetCities { get; }

        public uint OffsetLocations { get; }
        internal GeoBaseHeader(byte[] db)
        {
            Version = BitConverter.ToInt32(db, 0);

            var nameStartIndex = 4;
            Name = System.Text.Encoding.UTF8.GetString(db, nameStartIndex, NAME_LENGTH);

            var timestampStartIndex = nameStartIndex + NAME_LENGTH;
            Timestamp = BitConverter.ToUInt64(db, timestampStartIndex);

            var recordsStartIndex = timestampStartIndex + 8;
            Records = BitConverter.ToInt32(db, recordsStartIndex);

            var offsetRangesStartIndex = recordsStartIndex + 4;
            OffsetRanges = BitConverter.ToUInt32(db,offsetRangesStartIndex);

            var offsetCitiesStartIndex = offsetRangesStartIndex + 4;
            OffsetCities = BitConverter.ToUInt32(db, offsetCitiesStartIndex);

            var offsetLocationsStartIndex = offsetCitiesStartIndex + 4;
            OffsetLocations = BitConverter.ToUInt32(db, offsetLocationsStartIndex);

        }

    }
}
