using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck.Crawler
{
    public class ParticipantCrawler : ICrawler
    {
        private IScheduler scheduler;

        public ParticipantCrawler(IScheduler scheduler = null)
        {
            this.scheduler = scheduler;
        }

        public bool CanCrawl(CrawlResult result)
        {
            return true;
        }

        public IObservable<CrawlResult> Crawl(CrawlResult result)
        {
            var dom = result.DOM;

            var urls = dom["#results_competitor_list > a[onclick]"].Select(participant =>
            {
                //Will have to do some finagaling to get the onclick correct
                var clickAttr = participant.GetAttribute("onclick");
                var firstQuote = clickAttr.IndexOf('\'');
                var secondQuote = clickAttr.IndexOf('\'', firstQuote + 1);

                return clickAttr.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
            });

            return urls.ToObservable().SelectMany(url =>
            {
                return this.CreateCrawlerForUri(new Uri(url), scheduler: this.scheduler);
            }, (url, cq) => new CrawlResult(new Uri(url), cq));

        }
    }
}
