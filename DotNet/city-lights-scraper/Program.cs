using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.IO;

namespace DancingDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            var nameScraper = new CityLightsNameScraper();
            var eventScraper = new CityLightsEventScraper();

            var outputFile = new StreamWriter(File.OpenWrite(args[0]));

            nameScraper.Scrape(new Uri("http://www.citylightsball.com/pages/heat_lists/#"));

            nameScraper.Dancers
                .SelectMany((dancer, idx) => eventScraper.GetEvents(dancer), (dancer, idx1, events, idx2) =>
                {
                    return new KeyValuePair<string, Dancer>(events, dancer);
                })
                .Where(keyValue => !String.IsNullOrEmpty(keyValue.Key))
                .Do((n, idx) => { if (idx % 20 == 0) Console.WriteLine("{0} events process", idx); })
                .GroupBy(keyValue => keyValue.Key, keyValue => keyValue.Value)
                .SelectMany(group => group.ToList(), (group, dancers) => new Event { Name = group.Key, Dancers = dancers })
                .Do((n) => { }, () => { Console.WriteLine("Completed!"); })
                .Subscribe(outputFile.WriteLine, () => outputFile.Dispose());

            Console.ReadLine();

        }
    }
}
