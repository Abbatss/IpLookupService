using IPLookup.API.Services;
using IPLookup.API.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace IPLookup.API.Host.Controllers
{
    public class IpController : ApiController
    {
        private readonly IGeoDataBaseQuery _query;
        public IpController()
        { }
        public IpController(IGeoDataBaseQuery query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
        [System.Web.Mvc.HttpGet]
        public async Task<JsonResult<IEnumerable<LocationModel>>> GetLocation(string ip)
        {
            return Json(await _query.GetLocationsByIp(ip));
        }

    }
}
