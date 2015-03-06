using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck
{
    [JsonObject]
    public class Event
    {
        [JsonProperty("dancers")]
        public IList<Dancer> Dancers { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(String.Format("Name: {0}", Name));

            builder.AppendLine();

            foreach (var item in Dancers)
            {
                builder.AppendLine(item.Name);
            }

            builder.AppendLine();

            return builder.ToString();
        }

    }
}
