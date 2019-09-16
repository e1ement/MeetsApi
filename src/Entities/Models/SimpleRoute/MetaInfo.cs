using System;
using System.Collections.Generic;

namespace Entities.Models.SimpleRoute
{
    public class MetaInfo
    {
        public DateTime timestamp { get; set; }
        public string mapVersion { get; set; }
        public string moduleVersion { get; set; }
        public string interfaceVersion { get; set; }
        public List<string> availableMapVersion { get; set; }
    }
}
