using IPLookup.API.InMemoryDataBase;
using IPLookup.API.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPLookup.API.Services
{
    internal static class LocationExtentions
    {
        internal static LocationModel MapToModel(this LocationInfo location)
        {
            return new LocationModel()
            {
                City = location.City,
                Country = location.Country,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Organization = location.Organization,
                Postal = location.Postal,
                Region = location.Region,
            };
        }
    }
}
