using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Crawler
{
    public static class CrawlerExtensionMixins
    {
        public static IObservable<CQ> CreateCrawlerForUri(this ICrawler source, Uri uri, int retry = 3, IScheduler scheduler = null)
        {
            var crawl = Observable.Defer(() => CQ.CreateFromUrlAsync(uri.AbsoluteUri).ToObservable(scheduler))
            .Retry(retry)
            .Select(webResponse => webResponse.Dom);

            return crawl;
        }

    }
}
