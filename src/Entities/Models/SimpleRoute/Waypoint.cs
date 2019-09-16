namespace Entities.Models.SimpleRoute
{
    public class Waypoint
    {
        public string linkId { get; set; }
        public Position mappedPosition { get; set; }
        public Position originalPosition { get; set; }
        public string type { get; set; }
        public double spot { get; set; }
        public string sideOfStreet { get; set; }
        public string mappedRoadName { get; set; }
        public string label { get; set; }
        public int shapeIndex { get; set; }
        public string source { get; set; }
    }
}
