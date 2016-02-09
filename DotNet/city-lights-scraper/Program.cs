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
using System.Reactive.Concurrency;
using DancingDuck.Util;

namespace DancingDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            StartCrawl(args);
            Console.ReadLine();
        }

        private static void StartCrawl(string[] args)
        {
            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            var url = new Uri(options.InputUrl);

            var rootCrawler = new CompositeCrawler();
            var extractor = new ParticipantExtractor(new EventExtractor());
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
            };

            var writerObservable = Observable.Using(() => new JsonTextWriter(new StreamWriter(File.Open(options.Output, FileMode.Create))),
                writer => Observable.Return(writer));

            rootCrawler.SubCrawlers.Add(new ParticipantCrawler(new System.Reactive.Concurrency.EventLoopScheduler()));

            var extraction = rootCrawler.Crawl(url)
                .Do(x => Console.WriteLine("Extracting: " + x.Uri))
                .Take(10)
                .SelectMany(extractor.Extract)
                .Publish();

            var dances = extraction.SelectMany(dancer => dancer.Events)
                .Distinct(ev => ev.Name).ToList();

            var participants = extraction
                .Do(dancer => Console.WriteLine("Processed: {0} with {1} dances", dancer.Name, dancer.Events.Count))
                .ToList();

            Observable.Using(() => new TimerDisposable(), _ => dances.Zip(participants, (left, right) =>
                new Competition
                {
                    Dancers = right,
                    Events = left,
                    Version = 5
                }))
                .Do(_ => Console.WriteLine("Finished processing.  Starting write back"))
                .Subscribe(
                    body =>
                    {
                        using (var writer = new JsonTextWriter(new StreamWriter(File.Open(options.Output, FileMode.Create))))
                        {
                            serializer.Serialize(writer, body);
                        }
                    }, () => Console.WriteLine("Write back completed!"));

            extraction.Connect();
        }
    }
}
