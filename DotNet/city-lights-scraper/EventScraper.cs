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
            var domObservable = 
                CQ.CreateFromUrlAsync(dancer.Uri, new ServerConfig { Timeout = TimeSpan.FromSeconds(5) }).ToObservable()
                .Select(response => response.Dom);

            return DoGetEvents(domObservable);
                
        }

        protected abstract IObservable<string> DoGetEvents(IObservable<CQ> domObservable);

    }
}
