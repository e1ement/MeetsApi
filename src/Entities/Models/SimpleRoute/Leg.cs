using System.Collections.Generic;

namespace Entities.Models.SimpleRoute
{
    public class Leg
    {
        public Waypoint start { get; set; }
        public Waypoint end { get; set; }
        public int length { get; set; }
        public int travelTime { get; set; }
        public List<Maneuver> maneuver { get; set; }
    }
}
