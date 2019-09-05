using System;
using System.Collections.Generic;

namespace IPLookup.API.InMemoryDataBase
{
    public class CitiesIndex : IByValueBinarySearchObject
    {
        private const int CITIES_ROW_SIZE = 4;
        public uint LocationInfoIndex { get; private set; }

        public uint ItemIndex { get; private set; }
        public LocationInfo Location { get; private set; }


        public CitiesIndex(byte[] dataBase, uint index)
        {
            ItemIndex = index;
            var offset = new GeoBaseHeader(dataBase).OffsetCities;
            var startIndex = (int)(offset + (CITIES_ROW_SIZE) * index);
            if (dataBase.Length < startIndex + CITIES_ROW_SIZE)
                throw new InvalidOperationException("Can't read Cities index Row. Not enough data in DataBase.");

            LocationInfoIndex = BitConverter.ToUInt32(dataBase, startIndex);
            Location = LocationInfo.FromAbsolutePostition(dataBase, LocationInfoIndex);
        }
        public bool ContainsValue(string value)
        {
            return value?.TrimEnd('\0') == Location?.City;
        }

        public bool LessThan(string value)
        {
            var valueChars = value?.ToCharArray();
            if (string.IsNullOrEmpty(Location?.City))
            {
                return true;
            }
            for (int i = 0; i < valueChars?.Length && i < Location?.City?.Length; i++)
            {
                if (Location.City[i] == valueChars[i])
                {
                    continue;
                }
                return Location.City[i] < valueChars[i];
            }
            return Location?.City?.Length < valueChars?.Length;
        }

        public override bool Equals(object obj)
        {
            return obj is CitiesIndex index &&
                   LocationInfoIndex == index.LocationInfoIndex &&
                   ItemIndex == index.ItemIndex &&
                   EqualityComparer<LocationInfo>.Default.Equals(Location, index.Location);
        }

        public override int GetHashCode()
        {
            var hashCode = -334034214;
            hashCode = hashCode * -1521134295 + LocationInfoIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + ItemIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<LocationInfo>.Default.GetHashCode(Location);
            return hashCode;
        }
    }
}
