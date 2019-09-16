using Contracts;
using Entities.Dto.WayPoint;
using Entities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Meets.API.Controllers
{
    [Route("api/route")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly HereHelper _hereHelper;

        public RouteController(IRepositoryWrapper repository, IOptions<HereHelper> hereConfig)
        {
            _repository = repository;
            _hereHelper = hereConfig.Value;
        }

        /// <summary>
        /// Returns route from start to end wayPoint
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /route/getRoute
        ///     {
        ///         "startWayPoint": {
        ///             "wayPointX": 52.5,
        ///             "wayPointY": 13.4
        ///         },
        ///         "endWayPoint": {
        ///             "wayPointX": 52.5,
        ///             "wayPointY": 13.45
        ///         }
        ///     }
        /// </remarks>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("createRoute")]
        public async Task<IActionResult> CreateRoute([FromBody]WayPointsDto points)
        {
            var routes = await _repository.Route.GetRouteAsync(points, _hereHelper);

            return Ok(routes);
        }
    }
}