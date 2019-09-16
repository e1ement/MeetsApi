using Contracts;
using Entities;
using Entities.Models.SimpleRoute;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities.Dto.WayPoint;
using Entities.Helpers;
using Newtonsoft.Json;

namespace Repository
{
    public class RouteRepository : RepositoryBase<Route>, IRouteRepository
    {
        public RouteRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            
        }

        public async Task<RootObject> GetRouteAsync(WayPointsDto points, HereHelper here)
        {
            using (var client = new HttpClient())
            {
                RootObject route = null;
                var url = new StringBuilder();

                url.AppendFormat("https://route.api.here.com/routing/7.2/calculateroute.json?app_id={0}&app_code={1}&waypoint0=geo!{2},{3}&waypoint1=geo!{4},{5}&mode=fastest;car;traffic:disabled", 
                    here.AppId, 
                    here.AppCode, 
                    points.StartWayPoint.WayPointX.ToString("0.0#####", System.Globalization.CultureInfo.InvariantCulture), 
                    points.StartWayPoint.WayPointY.ToString("0.0#####", System.Globalization.CultureInfo.InvariantCulture), 
                    points.EndWayPoint.WayPointX.ToString("0.0#####", System.Globalization.CultureInfo.InvariantCulture), 
                    points.EndWayPoint.WayPointY.ToString("0.0#####", System.Globalization.CultureInfo.InvariantCulture));

                var response = await client.GetAsync(url.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStringAsync();
                    route = JsonConvert.DeserializeObject<RootObject>(stream);
                }

                return route;
            }
        }
    }
}
