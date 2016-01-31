using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingDuck.Crawler
{
    public interface IExtractor<T>
    {
        bool CanExtract(CrawlResult result);
        IEnumerable<T> Extract(CrawlResult result);
    }
}
