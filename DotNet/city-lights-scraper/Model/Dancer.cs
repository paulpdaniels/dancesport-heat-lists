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
            this.Events = new List<Event>();
            this.Id = Interlocked.Increment(ref s_index).ToString();
        }

        public string Id
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "events")]
        public List<Event> Events { get; set; }

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

        public List<string> Partners { get; set; }
    }


    [JsonObject]
    public class DancerView
    {
        private Dancer _dancer;

        public DancerView(Dancer dancer)
        {
            this._dancer = dancer;
        }

        [JsonProperty("$id")]
        public string Id
        {
            get
            {
                return _dancer.Id;
            }
        }

        [JsonProperty(PropertyName = "events")]
        public IEnumerable<string> Events
        {
            get
            {
                return _dancer.Events.Select(ev => ev.Id);
            }
        }

        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return _dancer.Name;
            }
        }

        [JsonProperty("partners")]
        public IEnumerable<string> Partners
        {
            get { return this._dancer.Partners; }
        }

        [JsonIgnore]
        public string Uri
        {
            get
            {
                return _dancer.Uri;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(String.Format("Name: {0}, Uri: {1}", Name, Uri));

            builder.AppendLine();

            return builder.ToString();
        }


        public static DancerView FromDancer(Dancer dancer)
        {
            return new DancerView(dancer);
        }
    }

}
