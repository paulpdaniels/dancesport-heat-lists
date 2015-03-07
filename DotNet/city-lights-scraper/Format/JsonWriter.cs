using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private Newtonsoft.Json.Linq.JTokenWriter _writer;
        private JObject _container;

        public JsonWriter(Stream output)
            : base(output)
        {
            this._serializer = new JsonSerializer() { Formatting = Formatting.Indented,  };
            this._metaWrapper = new MetaWrapper(1);
            var array = new JArray();
            _container = new JObject(new JProperty("version", 1), new JProperty("events", array));
            this._writer = new JTokenWriter(array);
        }


        public override void Write(IEnumerable<Event> events)
        {
            this._serializer.Serialize(this._writer, this._metaWrapper.Copy(events));
            this._writer.Flush();
        }

        public void Write(Event ev)
        {
            this._writer.WriteToken(new JTokenReader(JObject.FromObject(ev)));
        }

        public void Close()
        {
            this._writer.Close();

            using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(this.OutputStream)))
            {
                _serializer.Serialize(writer, this._container);
            }
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
