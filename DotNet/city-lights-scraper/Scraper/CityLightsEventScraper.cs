using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck.Scraper
{
    public class CityLightsEventScraper : EventScraper
    {
        protected override IEnumerable<string> DoGetEvents(CsQuery.CQ dom)
        {
            return dom["#heat_lists > table > tbody > tr:nth-child(n + 1) > td:nth-child(4) > p"].Select(ev => ev.InnerText);
        }
    }
}
