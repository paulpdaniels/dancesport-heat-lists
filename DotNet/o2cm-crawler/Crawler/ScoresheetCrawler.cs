using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck.Crawler
{
    public class ScoresheetCrawler : CompositeCrawler
    {
        public override bool CanCrawl(CrawlResult result)
        {
            return result.Uri.Host.Contains("results.o2cm.com");
        }

        public override IObservable<CrawlResult> Crawl(CrawlResult result)
        {
            return result.DOM["#placement > form > table:nth-child(2) > tbody > tr > td.h5b > a"].Select(domObj =>
            {
                return new Uri(result.Uri, domObj["href"]);
            })
            .Select(x => new CrawlResult(x, null))
            .ToObservable()
            .SelectMany(cr =>
            {
                return this.CreateCrawlerForUri(cr.Uri)
                    .Select(x => new CrawlResult(cr.Uri, x));
            });
        }
    }
}
