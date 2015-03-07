using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DancingDuck
{
    [JsonObject]
    public class Event
    {
        private static int s_index = 0;

        public Event()
        {
            Dancers = new List<int>();
            this.Id = Interlocked.Increment(ref s_index);
        }

        [JsonProperty("$id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dancers")]
        public IList<int> Dancers { get; private set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(String.Format("Name: {0}", Name));

            builder.AppendLine();

            return builder.ToString();
        }

    }
}
