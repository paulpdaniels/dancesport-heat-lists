using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DancingDuck.Crawler
{
    public class CrawlResult
    {
        public CrawlResult(Uri uri, CsQuery.CQ dom)
        {
            Uri = uri;
            DOM = dom;
        }


        public Uri Uri { get; private set; }
        public CsQuery.CQ DOM { get; private set; }
    }

    public interface ICrawler
    {
        bool CanCrawl(CrawlResult result);

        /// <summary>
        /// Allows us to recursively crawl a web page
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        IObservable<CrawlResult> Crawl(CrawlResult uri);
    }
}
