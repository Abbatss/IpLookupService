using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IPLookup.API.InMemoryDataBase;
using IPLookup.API.Services.Models;

namespace IPLookup.API.Services
{
    public class GeoDataBaseQuery : IGeoDataBaseQuery
    {

        public IInMemoryGeoDataBase Client { get; }

        public GeoDataBaseQuery(IInMemoryGeoDataBase client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<LocationModel> GetLocationsByIp(string ip)
        {
            var ipRange = await Client.SearchByValue<IPRange>(ip);
            if (ipRange != null)
            {
                var location = await Client.Get<Location>(ipRange.LocationIndex);
               return new LocationModel();
            }
            return null;
        }

        public Task<IEnumerable<LocationModel>> GetLocationsByCity(string city)
        {
            throw new NotImplementedException();
        }
    }
}
