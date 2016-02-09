using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck.Crawler
{
    public class CompositeCrawler : ICrawler
    {
        public CompositeCrawler()
        {
            this.SubCrawlers = new List<ICrawler>();
        }

        public virtual bool CanCrawl(CrawlResult result) { return true; }

        public virtual IObservable<CrawlResult> Crawl(CrawlResult result)
        {
            var crawl = this.CreateCrawlerForUri(result.Uri);

            return crawl.Select(cq => new CrawlResult(result.Uri, cq))
                .SelectMany(cr => this.SubCrawlers.Where(sub => sub.CanCrawl(cr)),
                (cr, sub) => sub.Crawl(cr))
                .Merge();
        }

        public IList<ICrawler> SubCrawlers { get; private set; }

        public IObservable<CrawlResult> Crawl(Uri uri)
        {
            return Crawl(new CrawlResult(uri, null));
        }
    }
}
