using Common;
using System;
using System.IO;

namespace IPLookup.API.InMemoryDataBase
{
    public class GeoBaseHeader
    {
        public const int HEADER_SIZE = 60;
        private const int NAME_LENGTH = 32;
        public int Version { get; private set; }
        public string Name { get; private set; }
        public ulong Timestamp { get; private set; }
        public int Records { get; private set; }
        public uint OffsetRanges { get; private set; }
        public uint OffsetCities { get; private set; }

        public uint OffsetLocations { get; private set; }
        internal GeoBaseHeader(byte[] db)
        {
            if (db is null)
            {
                throw new ArgumentNullException(nameof(db));
            }

            if (db.Length < HEADER_SIZE)
            {
                throw new InvalidOperationException("Can't read Header. Not enough data in DataBase.");
            }
            ParseHeaderData(db);

        }
        private void ParseHeaderData(byte[] db)
        {
            Version = BitConverter.ToInt32(db, 0);
            var nameStartIndex = 4;

            Name = GetName(db, nameStartIndex);

            var timestampStartIndex = nameStartIndex + NAME_LENGTH;
            Timestamp = BitConverter.ToUInt64(db, timestampStartIndex);

            var recordsStartIndex = timestampStartIndex + 8;
            Records = BitConverter.ToInt32(db, recordsStartIndex);

            var offsetRangesStartIndex = recordsStartIndex + 4;
            OffsetRanges = BitConverter.ToUInt32(db, offsetRangesStartIndex);

            var offsetCitiesStartIndex = offsetRangesStartIndex + 4;
            OffsetCities = BitConverter.ToUInt32(db, offsetCitiesStartIndex);

            var offsetLocationsStartIndex = offsetCitiesStartIndex + 4;
            OffsetLocations = BitConverter.ToUInt32(db, offsetLocationsStartIndex);
        }

        private string GetName(byte[] db, int nameStartIndex)
        {
            return db.ConvertToString(nameStartIndex, NAME_LENGTH);
        }
    }
}
