using System.Collections.Generic;

namespace Entities.Models.SimpleRoute
{
    public class Route
    {
        public List<Waypoint> waypoint { get; set; }
        public Mode mode { get; set; }
        public List<Leg> leg { get; set; }
        public Summary summary { get; set; }
    }
}
