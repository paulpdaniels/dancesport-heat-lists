using DancingDuck.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingDuck
{
    class FormExtractor : IExtractor<IList<string>>
    {
        public bool CanExtract(CrawlResult result)
        {
            return true;
        }

        public IEnumerable<IList<string>> Extract(CrawlResult result)
        {
            var title = result.DOM["body > form > table > tbody > tr:nth-child(3) > td.h4"];
            var headers = result.DOM["body > table:nth-child(2) > tbody > tr:nth-child(1) > td"];


            throw new NotImplementedException();
        }
    }
}
