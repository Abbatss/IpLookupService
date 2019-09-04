using System;

namespace IPLookup.API.InMemoryDataBase
{
    public class CitiesIndex : IByValueBinarySearchObject
    {
        private const int CITIES_ROW_SIZE = 4;
        public uint CitiesInfoIndex { get; private set; }

        public uint ItemIndex { get; private set; }
        public Location Location { get; private set; }


        public CitiesIndex(byte[] dataBase, uint index)
        {
            ItemIndex = index;
            var offset = new GeoBaseHeader(dataBase).OffsetCities;
            var startIndex = (int)(offset + (CITIES_ROW_SIZE) * index);
            if (dataBase.Length < startIndex + CITIES_ROW_SIZE)
                throw new InvalidOperationException("Can't read Cities index Row. Not enough data in DataBase.");

            CitiesInfoIndex = BitConverter.ToUInt32(dataBase, startIndex);
            Location = new Location(dataBase, CitiesInfoIndex);
        }
        public bool ContainsValue(string value)
        {
            return value == Location.City;
        }

        public bool LessThan(string value)
        {
            var valueChars = value.ToCharArray();
            if (string.IsNullOrEmpty(Location?.City))
            {
                return true;
            }
            for (int i = 0; i < valueChars.Length && i < Location?.City?.Length; i++)
            {
                if (Location.City[0] == valueChars[0])
                {
                    continue;
                }
                return Location.City[0] < valueChars[0];
            }
            return Location?.City?.Length < valueChars.Length;
        }
    }
}
