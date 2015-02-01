using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Format
{
    public class JsonWriter : BaseHeatWriter
    {
        private JsonSerializer _serializer;
        private MetaWrapper _metaWrapper;

        public JsonWriter(Stream output)
            : base(output)
        {
            this._serializer = new JsonSerializer() { Formatting = Formatting.Indented,  };
            this._metaWrapper = new MetaWrapper(1);
        }


        public override void Write(IEnumerable<Event> events)
        {
            var writer = new JsonTextWriter(new StreamWriter(this.OutputStream));
            this._serializer.Serialize(writer, this._metaWrapper.Copy(events));
            writer.Flush();
        }

        private class MetaWrapper
        {

            public MetaWrapper(int version)
            {
                this.Version = version;
            }

            [JsonProperty("version")]
            public int Version { get; private set;  }

            [JsonProperty("events")]
            public IEnumerable<Event> Events { get; private set; }

            public MetaWrapper Copy(IEnumerable<Event> events)
            {
                return new MetaWrapper(this.Version) { Events = events };
            }
        }
    }
}
