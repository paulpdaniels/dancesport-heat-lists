﻿using CsQuery;
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
        internal CrawlResult(Uri uri, CsQuery.CQ dom)
        {
            Uri = uri;
            DOM = dom;
        }


        public Uri Uri { get; private set; }
        public CsQuery.CQ DOM { get; private set; }
    }

    public interface IGenericCrawler
    {
        bool CanCrawl(CrawlResult result);

        /// <summary>
        /// Allows us to recursively crawl a web page
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        IObservable<CrawlResult> Crawl(CrawlResult uri);
    }

    public class RootCrawler : IGenericCrawler
    {
        public RootCrawler()
        {
            this.SubCrawlers = new List<IGenericCrawler>();
        }

        public bool CanCrawl(CrawlResult result) { return true; }

        public IObservable<CrawlResult> Crawl(CrawlResult result)
        {
            var crawl = this.CreateCrawlerForUri(result.Uri);

            return crawl.Select(cq => new CrawlResult(result.Uri, cq))
                .SelectMany(cr => this.SubCrawlers.Where(sub => sub.CanCrawl(cr)),
                (cr, sub) => sub.Crawl(cr))
                .Merge();
        }

        public IList<IGenericCrawler> SubCrawlers { get; private set; }

        public IObservable<CrawlResult> Crawl(Uri uri)
        {
            return Crawl(new CrawlResult(uri, null));
        }
    }

    public class ParticipantCrawler : IGenericCrawler
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
