using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck.Scraper
{
    public class CityLightsNameScraper : NameScraper
    {

        protected override IObservable<Dancer> DoScrape(IObservable<CsQuery.CQ> domObservable)
        {
            return
                domObservable.SelectMany(dom => dom["#results_competitor_list > a[onclick]"], (dom, competitor) =>
                {
                    var dancer = new Dancer();
                    //Will have to do some finagaling to get the onclick correct
                    var clickAttr = competitor.GetAttribute("onclick");
                    var firstQuote = clickAttr.IndexOf('\'');
                    var secondQuote = clickAttr.IndexOf('\'', firstQuote + 1);

                    dancer.Name = competitor.InnerText;
                    dancer.Uri = clickAttr.Substring(firstQuote + 1, secondQuote - firstQuote - 1);

                    return dancer;
                });
        }
    }
}
