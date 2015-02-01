using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingDuck.Format
{
    public interface IHeatWriter : IDisposable
    {

        void Write(IEnumerable<Event> events);

    }
}
