using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck.Crawler
{
    public class CompetitionCrawler : CompositeCrawler
    {
        public bool CanCrawl(CrawlResult result)
        {
            return result.Uri.Host.Contains("results.o2cm.com");
        }

        public override IObservable<CrawlResult> Crawl(CrawlResult result)
        {
            return Observable.If(() => Extractor.CanExtract(result),
                Extractor.Extract(result).ToObservable()
                .Do(x => Console.WriteLine("Crawling: " + x))
                .SelectMany(uri => base.Crawl(new CrawlResult(uri, null))));
        }

        public IExtractor<Uri> Extractor { get; set; }
    }
}
