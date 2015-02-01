using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Format
{
    public abstract class BaseHeatWriter : IHeatWriter
    {
        protected Stream OutputStream { get; private set; }

        public BaseHeatWriter(Stream output)
        {
            this.OutputStream = output;
        }

        public abstract void Write(IEnumerable<Event> events);

        public void Dispose()
        {
            this.OutputStream.Dispose();
        }
    }
}
