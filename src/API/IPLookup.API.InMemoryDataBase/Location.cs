using System;
using System.Collections.Generic;
using System.Text;

namespace IPLookup.API.InMemoryDataBase
{
    public class Location : IByValueBinarySearchObject
    {
        public string Country { get; private set; }
        public string Region { get; private set; }
        public string Postal { get; private set; }
        public string City { get; private set; }

        public string Organization { get; private set; }

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }
        public bool ContainsValue(string value)
        {
            return value == City;
        }

        public bool Less(string value)
        {
            return value.ToCharArray()[0] > City[0];
        }
    }
}
