using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DancingDuck
{
    [JsonObject]
    public class Dancer
    {
        private static int s_index = 0;

        public Dancer()
        {
            this.Events = new List<int>();
            this.Id = Interlocked.Increment(ref s_index);
        }

        [JsonProperty(PropertyName = "events")]
        public List<int> Events { get; private set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("$id")]
        public int Id { get; private set; }

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
