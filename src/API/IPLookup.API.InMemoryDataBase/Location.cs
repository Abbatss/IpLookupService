using Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace IPLookup.API.InMemoryDataBase
{
    public class LocationInfo
    {
        private const int LOCATION_ROW_SIZE = 96;
        public const int CITY_NAME_OFFSET = 36;
        private const int COUNTRY_SIZE = 8;
        private const int REGION_SIZE = 12;
        private const int POSTAL_SIZE = 12;
        private const int CITY_SIZE = 24;
        private const int ORGANIZATION_SIZE = 32;

        public string Country { get; private set; }
        public string Region { get; private set; }
        public string Postal { get; private set; }
        public string City { get; private set; }

        public string Organization { get; private set; }

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }

        public LocationInfo(byte[] dataBase, int index) : this(dataBase, new GeoBaseHeader(dataBase).OffsetLocations, index)
        {

        }
        private LocationInfo(byte[] dataBase, uint offset, int index)
        {

            if (dataBase is null)
            {
                throw new ArgumentNullException(nameof(dataBase));
            }
            var startIndex = (int)(offset + (LOCATION_ROW_SIZE) * index);
            if (dataBase.Length < startIndex + LOCATION_ROW_SIZE)
                throw new InvalidOperationException("Can't read Location index Row. Not enough data in DataBase.");
            ParseRowData(dataBase, startIndex);
        }
        internal static LocationInfo FromAbsolutePostition(byte[] dataBase, uint LocationInfoIndex)
        {
            if (LocationInfoIndex >= new GeoBaseHeader(dataBase).OffsetLocations)
            {
                // index points to City name.
                return new LocationInfo(dataBase, LocationInfoIndex - CITY_NAME_OFFSET, 0);
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LocationInfo);
        }

        public bool Equals(LocationInfo other)
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
        private void ParseRowData(byte[] db, int startIndex)
        {
            Country = db.ConvertToString(startIndex, COUNTRY_SIZE).TrimEnd('\0');
            var regionIndex = startIndex + COUNTRY_SIZE;
            Region = db.ConvertToString(regionIndex, REGION_SIZE).TrimEnd('\0');
            var postalIndex = regionIndex + REGION_SIZE;
            Postal = db.ConvertToString(postalIndex, POSTAL_SIZE).TrimEnd('\0');
            var cityIndex = postalIndex + POSTAL_SIZE;
            City = db.ConvertToString(cityIndex, CITY_SIZE).TrimEnd('\0');
            var organizationIndex = cityIndex + CITY_SIZE;
            Organization = db.ConvertToString(organizationIndex, ORGANIZATION_SIZE).TrimEnd('\0');

            Latitude = BitConverter.ToSingle(db, organizationIndex + 4);
            Longitude = BitConverter.ToSingle(db, organizationIndex + 8);
        }
    }
}
