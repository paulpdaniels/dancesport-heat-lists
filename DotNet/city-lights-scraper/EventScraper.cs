using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using CsQuery.Web;

namespace DancingDuck
{
    public abstract class EventScraper
    {

        public IObservable<string> GetEvents(Dancer dancer)
        {
            return  
                CQ.CreateFromUrlAsync(dancer.Uri, new ServerConfig { Timeout = TimeSpan.FromSeconds(5) }).ToObservable()
                .Select(response => response.Dom)
                .SelectMany(this.DoGetEvents);
        }

        protected abstract IEnumerable<string> DoGetEvents(CQ dom);

    }
}
