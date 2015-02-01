using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Net.Http;
using CsQuery;

namespace DancingDuck
{
    public abstract class NameScraper
    {
        private ReplaySubject<Dancer> _dancers = new ReplaySubject<Dancer>();

        public IObservable<Dancer> Dancers
        {
            get { return this._dancers.AsObservable(); }
        }

        public void Scrape(Uri requestUri)
        {
            var domObservable = CQ.CreateFromUrlAsync(requestUri.AbsoluteUri)
                .ToObservable()
                .Select(response => response.Dom);

            
            DoScrape(domObservable).Subscribe(this._dancers);
        }

        protected abstract IObservable<Dancer> DoScrape(IObservable<CQ> domObservable);

    }
}
