using System.Collections.Generic;

namespace Entities.Models.SimpleRoute
{
    public class Response
    {
        public MetaInfo metaInfo { get; set; }
        public List<Route> route { get; set; }
        public string language { get; set; }
    }
}
