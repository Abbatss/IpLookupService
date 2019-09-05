using IPLookup.API.Services;
using IPLookup.API.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using Common;
namespace IPLookup.API.Host.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class IpController : ApiController
    {
        private readonly IGeoDataBaseQuery _query;
        public IpController(IGeoDataBaseQuery query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
      
        [System.Web.Mvc.HttpGet]
        public async Task<JsonResult<LocationModel>> GetLocation(string ip)
        {
            if (!ip.IsValidIp())
            {
                return Json<LocationModel>(null);
            }
            return Json(await _query.GetLocationByIp(ip));
        }
        [System.Web.Mvc.HttpGet]
        public async Task<JsonResult<List<LocationModel>>> GetLocations(string city)
        {
            return Json(await _query.GetLocationsByCity(city));
        }
    }
}
