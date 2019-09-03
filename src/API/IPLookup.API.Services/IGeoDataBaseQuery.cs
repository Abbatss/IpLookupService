using IPLookup.API.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPLookup.API.Services
{
    public interface IGeoDataBaseQuery
    {
        Task<LocationModel> GetLocationsByIp(string ip);
        Task<IEnumerable<LocationModel>> GetLocationsByCity(string city);
    }
}
