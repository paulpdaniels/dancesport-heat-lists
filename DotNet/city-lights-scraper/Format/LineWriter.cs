using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Format
{
    public class LineWriter : BaseHeatWriter
    {
        public LineWriter(Stream outputStream)
            : base(outputStream)
        {
        }

        public override async void Write(IEnumerable<Event> events)
        {
            StreamWriter writer = new StreamWriter(this.OutputStream);
            writer.Write(String.Format("Version: {0}", 1));

            foreach (var item in events)
            {
                writer.WriteLine("event: {0}", item.Name);
                foreach (var dancer in item.Dancers)
                {
                    writer.WriteLine("\t{0}", dancer.Name);
                }
            }

            await writer.FlushAsync();

        }
    }
}
