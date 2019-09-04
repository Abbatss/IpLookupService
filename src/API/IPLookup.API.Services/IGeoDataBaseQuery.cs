using IPLookup.API.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPLookup.API.Services
{
    public interface IGeoDataBaseQuery
    {
        Task<LocationModel> GetLocationByIp(string ip);
        Task<List<LocationModel>> GetLocationsByCity(string city);
    }
}
