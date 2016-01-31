using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DancingDuck;

namespace DancingDuck.Crawler
{
    public class EventExtractor : IExtractor<Event>
    {
        private static readonly Regex regex = new Regex("\\s{2,}");

        public bool CanExtract(CrawlResult result)
        {
            return true;
        }

        public IEnumerable<Event> Extract(CrawlResult result)
        {
            var dom = result.DOM;

            var selector = dom["#heat_lists > table > tbody > tr:nth-child(n + 1) > td:nth-child(4) > p"];
            var timeSelector = dom["#heat_lists > table > tbody > tr > td.heatlist_datetime > p"];
            var sessionSelector = dom["#heat_lists > table > tbody > tr > td[valign='top']:nth-child(1) > p"];
            var heatSelector = dom["#heat_lists > table > tbody > tr > td[valign='top']:nth-child(2) > p"];

            var events = from eventNames in selector
                         let name = regex.Replace(eventNames.InnerText.Trim(), " ")
                         select new Event { Name = name };

            var times = from time in timeSelector
                        select time.InnerText.Trim();

            var sessions = from session in sessionSelector
                           select session.InnerText.Trim();

            var heats = from heat in heatSelector
                        select heat.InnerText.Trim();

            return events.Zip(times, sessions, heats, (ev, t, s, h) =>
            {
                ev.Session = s;
                ev.Heat = h;
                ev.Time = t;
                return ev;
            });
        }
    }
}
