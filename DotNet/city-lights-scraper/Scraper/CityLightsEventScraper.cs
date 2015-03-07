using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Text.RegularExpressions;

namespace DancingDuck.Scraper
{
    public class CityLightsEventScraper : EventScraper
    {
        private Regex regex = new Regex("\\s{2,}");

        protected override IEnumerable<Event> DoGetEvents(CsQuery.CQ dom)
        {
            return dom["#heat_lists > table > tbody > tr:nth-child(n + 1) > td:nth-child(4) > p"].Select(ev => new Event { 
                Name = regex.Replace(ev.InnerText.Trim(), " ")
            });
        }
    }
}
