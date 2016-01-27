using DancingDuck.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DancingDuck
{
    [JsonObject]
    public class Event
    {

        public Event()
        {

        }

        [JsonProperty("$id")]
        public string Id
        {
            get
            {
                return ShaHelpers.ComputeSha1Hash(this.Name, 6);
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

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
