using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Model
{
    
    public class Competition
    {


        [JsonProperty("version")]
        public short Version { get; set; }

        [JsonProperty("dancers")]
        public IList<DancerView> Dancers { get; set; }

        [JsonProperty("events")]
        public IList<Event> Events { get; set; }

    }
}
