using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPLookup.API.Services.Models;

namespace IPLookup.API.Services
{
    public class GeoDataBaseQuery : IGeoDataBaseQuery
    {
        Random random = new Random();
        private readonly IEnumerable<LocationModel> list;
        public GeoDataBaseQuery()
        {
            list = new List<LocationModel>() { new LocationModel() { City = random.Next(1000).ToString(), Latitude = 1, Longitude = 1.2344f },
                new LocationModel() { City = random.Next(1000).ToString(), Latitude = 2, Longitude = 2.2344f } }.AsEnumerable();
        }

        public Task<IEnumerable<LocationModel>> GetLocationsByIp(string ip)
        {
            return Task.FromResult(list);
        }

        public Task<IEnumerable<LocationModel>> GetLocationsByCity(string city)
        {
            throw new NotImplementedException();
        }
    }
}
