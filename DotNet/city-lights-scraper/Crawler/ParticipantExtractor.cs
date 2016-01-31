using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DancingDuck.Crawler
{
    public class ParticipantExtractor : IExtractor<Dancer>
    {
        private IExtractor<Event> eventExtractor;

        public ParticipantExtractor(IExtractor<Event> eventExtractor)
        {
            this.eventExtractor = eventExtractor;
        } 

        public bool CanExtract(CrawlResult result)
        {
            return true;
        }

        public IEnumerable<Dancer> Extract(CrawlResult crawlResult)
        {
            var dom = crawlResult.DOM;
            var uri = crawlResult.Uri;

            var dancerName = dom["#header > span"].Select(x => x.InnerText.Trim());
            var partnerNames = dom["#heat_lists > table > tbody > tr > td[colspan='4'] > p"]
                .Select(pn => {
                    var trimmedText = pn.InnerText.Trim(':', ' ');
                    var withIndex = trimmedText.IndexOf("with");
                    var minusWith = trimmedText.Substring(withIndex + 4).Trim();
                    return minusWith;
                });

            return from name in dancerName
                   select new Dancer
                   {
                       Name = name,
                       Uri = uri.AbsoluteUri,
                       Events = eventExtractor.Extract(crawlResult).ToList(),
                       //Events = eventNames.Select(ev => new Event {  Name = ev }).ToList(),
                       Partners = partnerNames.Distinct().ToList()
                   };

        }

    }
}
