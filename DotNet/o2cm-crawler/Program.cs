using DancingDuck.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlO2CM(args);
            Console.ReadLine();
        }

        private static void CrawlO2CM(string [] args)
        {

            var rootCrawler = new CompositeCrawler();

            var competitionCrawler = new CompetitionCrawler
            {
                Extractor = new CompetitionExtractor()
            };
            competitionCrawler.SubCrawlers.Add(new ScoresheetCrawler());
            rootCrawler.SubCrawlers.Add(competitionCrawler);

            var formExtractor = new FormExtractor();

            rootCrawler.Crawl(new Uri("http://results.o2cm.com/"))
                .SelectMany(formExtractor.Extract)
                .Subscribe(x =>
                {
                    Console.WriteLine("Read event: " + x);
                });

        }
    }
}
