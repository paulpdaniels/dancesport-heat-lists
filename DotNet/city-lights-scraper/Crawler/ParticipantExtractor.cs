using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DancingDuck.Crawler
{
    public class ParticipantExtractor
    {
        private static readonly Regex regex = new Regex("\\s{2,}");

        public bool CanExtract(CrawlResult result)
        {
            return true;
        }

        public IEnumerable<Dancer> Extract(CrawlResult crawlResult)
        {
            var dom = crawlResult.DOM;
            var uri = crawlResult.Uri;

            var dancerName = dom["#header > span"].Select(x => x.InnerText);
            var partnerNames = dom["#heat_lists > table > tbody > tr > td[colspan='4'] > p"]
                .Select(pn => {
                    var trimmedText = pn.InnerText.Trim(':', ' ');
                    var withIndex = trimmedText.IndexOf("with");
                    var minusWith = trimmedText.Substring(withIndex + 4).Trim();
                    var tokens = minusWith.Split(new char[] {' '}, 2);
                    return tokens[1] + ", " + tokens[0];
                });
            var eventNames = dom["#heat_lists > table > tbody > tr:nth-child(n + 1) > td:nth-child(4) > p"].Select(
                ev => regex.Replace(ev.InnerText.Trim(), " "));

            return from name in dancerName
                   select new Dancer
                   {
                       Name = name,
                       Uri = uri.AbsoluteUri,
                       Events = eventNames.Select(ev => new Event {  Name = ev }).ToList(),
                       Partners = partnerNames.Distinct().ToList()
                   };

        }

    }
}
