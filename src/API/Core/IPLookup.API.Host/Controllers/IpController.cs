using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Common;
using IPLookup.API.Services;

namespace Core.IPLookup.API.Host.Controllers
{
    [ApiController]
    public class IpController : ControllerBase
    {
        internal const string GetLocationByIPPath = "/api/ip/location";
        internal const string GetLocationsByCityPath = "/api/ip/locations";

        public IGeoDataBaseQuery Query { get; }

        public IpController(IGeoDataBaseQuery query)
        {
            Query = query ?? throw new System.ArgumentNullException(nameof(query));
        }

        [HttpGet("/health")]
        [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Get()
        {
            var answer = new
            {
                status = "Healthy",
                checks = new string[0],
            };

            return Ok(answer);
        }

        /// <summary>
        /// Get location by Ip
        /// </summary>
        /// <param name="ip">ip</param>
        [HttpGet(GetLocationByIPPath)]
        [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> GetLocationByIp([FromQuery]  string ip)
        {
            if (!ip.IsValidIp())
            {
                return Ok();
            }
            return Ok(await Query.GetLocationByIp(ip));
        }

        /// <summary>
        /// Get locations by city
        /// </summary>
        /// <param name="city">city name</param>
        [HttpGet(GetLocationsByCityPath)]
        [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> GetLocationsByCity([FromQuery]  string city)
        {
            return Ok(await Query.GetLocationsByCity(city));
        }
    }
}