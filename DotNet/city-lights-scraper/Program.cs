using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.IO;
//using DancingDuck.Format;
using System.Reactive;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading;
using DancingDuck.Crawler;

namespace DancingDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessScrape(args);
            Console.ReadLine();
        }

        private static void ProcessScrape(string [] args)
        {
            var rootCrawler = new RootCrawler();
            var extractor = new ParticipantExtractor();

            rootCrawler.SubCrawlers.Add(new ParticipantCrawler());

            var extraction = rootCrawler.Crawl(new Uri("http://www.citylightsball.com/pages/heat_lists/"))
                .Do(x => Console.WriteLine("Extracting: " + x.Uri))
                .SelectMany(extractor.Extract)
                .Publish();

            var dances = extraction.SelectMany(dancer => dancer.Events)
                .Distinct(ev => ev.Name).Aggregate(new JArray(), (root, dance) =>
                {
                    root.Add(JObject.FromObject(dance));
                    return root;
                });

            var participants = extraction
                .Do(dancer => Console.WriteLine("Processed: {0} with {1} dances", dancer.Name, dancer.Events.Count))
                .Select(DancerView.FromDancer)
                .Aggregate(new JArray(), (root, dancer) =>
                {
                    root.Add(JObject.FromObject(dancer));
                    return root;
                });

            dances.Zip(participants, (left, right) => 
                new JObject(new JProperty("version", 4),
                    new JProperty("dancers", right),
                    new JProperty("events", left)))
                .Subscribe(body => {
                    Console.WriteLine("Finished processing.  Starting write back");

                    using (var writer = new JsonTextWriter(new StreamWriter(File.Open(args[0], FileMode.Create))))
                    {
                        new JsonSerializer()
                        {
                            Formatting = Formatting.Indented,
                            PreserveReferencesHandling = PreserveReferencesHandling.Arrays
                        }
                        .Serialize(writer, body);

                        Console.WriteLine("Write back completed!");
                    }
                });

            extraction.Connect();
        }
    }
}
