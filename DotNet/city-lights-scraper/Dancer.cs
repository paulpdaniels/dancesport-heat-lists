using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingDuck
{
    [JsonObject]
    public class Dancer
    {
        public Dancer()
        {
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string Uri { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(String.Format("Name: {0}, Uri: {1}", Name, Uri));

            builder.AppendLine();

            return builder.ToString();
        }

    }
}
