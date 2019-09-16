using System.Collections.Generic;

namespace Entities.Models.SimpleRoute
{
    public class Mode
    {
        public string type { get; set; }
        public List<string> transportModes { get; set; }
        public string trafficMode { get; set; }
        public List<object> feature { get; set; }
    }
}
