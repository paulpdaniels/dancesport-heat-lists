using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Util
{
    public class TimerDisposable : IDisposable
    {
        private bool isDisposed;
        private DateTime _startedAt;

        public TimerDisposable()
        {
            _startedAt = DateTime.Now;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                var elapsed = DateTime.Now.Subtract(_startedAt);
                Console.WriteLine("==== Total Time: {0} =====", elapsed.TotalMilliseconds);
                isDisposed = true;
            }
        }
    }
}
