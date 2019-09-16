using System.Threading.Tasks;
using Entities.Dto.WayPoint;
using Entities.Helpers;
using Entities.Models.SimpleRoute;

namespace Contracts
{
    public interface IRouteRepository
    {
        Task<RootObject> GetRouteAsync(WayPointsDto points, HereHelper here);
    }
}
