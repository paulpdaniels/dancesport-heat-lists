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

        public async Task<IEnumerable<Event>> GetEvents(Dancer dancer)
        {
            ICsqWebResponse webResponse = await CQ.CreateFromUrlAsync(dancer.Uri, new ServerConfig { Timeout = TimeSpan.FromSeconds(5) });

            return DoGetEvents(webResponse.Dom);
        }

        protected abstract IEnumerable<Event> DoGetEvents(CQ dom);

    }
}
