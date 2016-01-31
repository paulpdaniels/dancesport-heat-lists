using DancingDuck.Util;
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
        private static readonly char[] splitTokens = new char[] { ',' };
        public static string ComputeNameHash(string name)
        {
            var nameParts = name.Split(splitTokens, 2);
            var normalizedName = String.Join(" ", nameParts.Reverse()).Trim();
            return ShaHelpers.ComputeSha1Hash(normalizedName, 6);
        }

        public Dancer()
        {
            this.Events = new List<Event>();
        }

        [JsonProperty("$id")]
        public string Id
        {
            get
            {
                return ComputeNameHash(this.Name);
            }
        }

        
        [JsonIgnore]
        public List<Event> Events { get; set; }

        [JsonProperty("events")]
        public IEnumerable<string> EventsHelper
        {
            get
            {
                return from ev in Events
                       select ev.Id;
            }
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

        [JsonIgnore]
        public List<string> Partners { get; set; }

        [JsonProperty("partners")]
        public IEnumerable<string> PartnersHelper
        {
            get
            {
                return Partners.Select(x => ComputeNameHash(x));
            }
        }
    }
}
