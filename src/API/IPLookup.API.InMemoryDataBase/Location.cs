using System;
using System.Collections.Generic;

namespace IPLookup.API.InMemoryDataBase
{
    public class Location 
    {
        private const int LOCATION_ROW_SIZE = 96;
        public string Country { get; private set; }
        public string Region { get; private set; }
        public string Postal { get; private set; }
        public string City { get; private set; }

        public string Organization { get; private set; }

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }

        public Location(byte[] dataBase, uint index)
        {
            var offset = new GeoBaseHeader(dataBase).OffsetLocations;
            var startIndex = (int)(offset + (LOCATION_ROW_SIZE) * index);
            if (dataBase.Length < startIndex + LOCATION_ROW_SIZE)
                throw new InvalidOperationException("Can't read Cities index Row. Not enough data in DataBase.");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public bool Equals(Location other)
        {
            return other != null &&
                   Country == other.Country &&
                   Region == other.Region &&
                   Postal == other.Postal &&
                   City == other.City &&
                   Organization == other.Organization &&
                   Latitude == other.Latitude &&
                   Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            var hashCode = 291169015;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Region);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Postal);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Organization);
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            return hashCode;
        }
    }
}
