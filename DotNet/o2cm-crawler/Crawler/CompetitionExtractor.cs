using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Crawler
{
    class CompetitionExtractor : IExtractor<Uri>
    {
        public bool CanExtract(CrawlResult result)
        {
            return result.Uri.Host.Contains("results.o2cm.com") &&
                result.DOM["#main_tbl"].Any();
        }

        public IEnumerable<Uri> Extract(CrawlResult result)
        {
            Console.WriteLine("Extracting: " + result.Uri);
            return result.DOM["#main_tbl > tbody > tr > td:nth-child(2) > a"].Select(domObj =>
            {
                return new Uri(result.Uri, domObj["href"]);
            });
        }
    }
}
