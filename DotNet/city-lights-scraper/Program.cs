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
using DancingDuck.Model;

namespace DancingDuck
{
    class Program
    {

        private static readonly IEnumerable<string> defaultPaths = new string[] { 
            "crawl.txt", "crawl.json"
        };

        static void Main(string[] args)
        {
            StartCrawl(args);
            Console.ReadLine();
        }

        private static void StartCrawl(string [] args)
        {
            var config = defaultPaths.First(File.Exists);

            var url = File.ReadLines(config).First().Split(' ')[1];
            
            var rootCrawler = new RootCrawler();
            var extractor = new ParticipantExtractor(new EventExtractor());

            rootCrawler.SubCrawlers.Add(new ParticipantCrawler());

            var extraction = rootCrawler.Crawl(new Uri(url))
                .Do(x => Console.WriteLine("Extracting: " + x.Uri))
                .SelectMany(extractor.Extract)
                .Publish();

            var dances = extraction.SelectMany(dancer => dancer.Events)
                .Distinct(ev => ev.Name).ToList();

            var participants = extraction
                .Do(dancer => Console.WriteLine("Processed: {0} with {1} dances", dancer.Name, dancer.Events.Count))
                .Select(DancerView.FromDancer).ToList();

            dances.Zip(participants, (left, right) => 
                new Competition {
                    Dancers = right,
                    Events = left,
                    Version = 4
                })
                .Subscribe(body => {
                    Console.WriteLine("Finished processing.  Starting write back");

                    using (var writer = new JsonTextWriter(new StreamWriter(File.Open(args[0], FileMode.Create))))
                    {
                        new JsonSerializer()
                        {
                            Formatting = Formatting.Indented,
                        }
                        .Serialize(writer, body);

                        Console.WriteLine("Write back completed!");
                    }
                });

            extraction.Connect();
        }
    }
}
