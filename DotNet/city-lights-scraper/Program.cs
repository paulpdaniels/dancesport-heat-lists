using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.IO;
using DancingDuck.Scraper;
//using DancingDuck.Format;
using System.Reactive;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading;

namespace DancingDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            var nameScraper = new CityLightsNameScraper();
            var eventsCrawler = new CityLightsEventScraper();

            //var writer = new JsonWriter(File.Open(args[0], FileMode.Create));

            //var writingObserver = Observer.Create<Event>(ev =>
            //{
            //    writer.Write(ev);
            //},
            //() => { 
            //    writer.Close(); 
            //});

            var eventDictionary = new ConcurrentDictionary<string, Event>();
            var dancerDictionary = new ConcurrentDictionary<string, Dancer>();

            int index = 0;

            nameScraper.Scrape(new Uri("http://usnationals.byudancesport.com/pages/heat_lists/Default.asp#"));

            nameScraper.Dancers.Take(10)
                .Do(dancer =>
                {
                    Console.WriteLine("Processing dancer: {0}", Interlocked.Increment(ref index));

                    dancerDictionary.TryAdd(dancer.Name, dancer);
                })
                .SelectMany(dancer => eventsCrawler.GetEvents(dancer), (dancer, coll) =>
                {

                    var events = from ev in coll
                                 where !String.IsNullOrWhiteSpace(ev.Name)
                                 select ev;

                    dancer.Events.AddRange(events.Select(e => e.Id));

                    return events;
                })
                .Subscribe(events =>
            {
                foreach (var ev in events)
                    eventDictionary.TryAdd(ev.Name, ev);
            }, e => {

                Console.WriteLine(e);

            }, () =>
            {
                try
                {
                    Console.WriteLine("Finished processing.  Starting write back");
                    var dancers = JArray.FromObject(dancerDictionary.Values);
                    var events = JObject.FromObject(eventDictionary);
                    var obj = new JObject(new JProperty("version", 2),
                        new JProperty("dancers", dancers),
                        new JProperty("events", events));

                    using (var writer = new JsonTextWriter(new StreamWriter(File.Open(args[0], FileMode.Create))))
                    {
                        new JsonSerializer() { Formatting = Formatting.Indented }.Serialize(writer, obj);
                        Console.WriteLine("Write back completed!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Problem serializing");
                }
            });

            //var events = nameScraper.Dancers
            //    .Take(20)
            //    .SelectMany((dancer, idx) => eventScraper.GetEvents(dancer),
            //    (dancer, idx1, events2, idx2) => new KeyValuePair<string, Dancer>(events2, dancer))
            //    .Where(keyValue => !String.IsNullOrEmpty(keyValue.Key))
            //    .Do((n, idx) => { if (idx % 20 == 0) Console.WriteLine("{0} pairs processed", idx); })
            //    .GroupBy(keyValue => keyValue.Key, keyValue => keyValue.Value)

            //    .SelectMany(group => group.ToList(), (group, dancers) => new Event { Name = group.Key, Dancers = dancers })
            //    .Do(_ => { }, () => { Console.WriteLine("Completed!"); })
            //    .Subscribe(writingObserver);


            //using (var heatWriter = new JsonWriter(File.Open(args[0], FileMode.Create)))
            //    heatWriter.Write(events.ToEnumerable());

            Console.ReadLine();

        }
    }
}
