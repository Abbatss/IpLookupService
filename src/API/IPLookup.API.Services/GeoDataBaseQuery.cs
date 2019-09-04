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

        public async Task<LocationModel> GetLocationByIp(string ip)
        {
            var ipRange = await Client.SearchFirstItemByValue<IPRange>(ip);
            if (ipRange != null)
            {
                var location = await Client.Get<Location>(ipRange.LocationIndex);
                return location?.MapToModel();
            }
            return null;
        }
        //IAsyncEnumerable will be much better but only after c# 8 
        public async Task<List<LocationModel>> GetLocationsByCity(string city)
        {
            var list = new List<LocationModel>();
            var locationIndex = await Client.SearchFirstItemByValue<CitiesIndex>(city);
            while (locationIndex?.Location?.City == city)
            {
                list.Add(locationIndex.Location.MapToModel());
                locationIndex = await Client.Get<CitiesIndex>((int)locationIndex.ItemIndex + 1);
            }
            return list;
        }
    }
}
