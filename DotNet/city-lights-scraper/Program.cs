﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.IO;
using DancingDuck.Scraper;
using DancingDuck.Format;
using System.Reactive;

namespace DancingDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            var nameScraper = new CityLightsNameScraper();
            var eventScraper = new CityLightsEventScraper();

            var writer = new JsonWriter(File.Open(args[0], FileMode.Create));

            var writingObserver = Observer.Create<Event>(ev =>
            {
                writer.Write(ev);
            },
            () => { 
                writer.Close(); 
            });

            nameScraper.Scrape(new Uri("http://usnationals.byudancesport.com/pages/heat_lists/Default.asp#"));

            var events = nameScraper.Dancers
                .Take(20)
                .SelectMany((dancer, idx) => eventScraper.GetEvents(dancer),
                (dancer, idx1, events2, idx2) => new KeyValuePair<string, Dancer>(events2, dancer))
                .Where(keyValue => !String.IsNullOrEmpty(keyValue.Key))
                .Do((n, idx) => { if (idx % 20 == 0) Console.WriteLine("{0} pairs processed", idx); })
                .GroupBy(keyValue => keyValue.Key, keyValue => keyValue.Value)

                .SelectMany(group => group.ToList(), (group, dancers) => new Event { Name = group.Key, Dancers = dancers })
                .Do(_ => { }, () => { Console.WriteLine("Completed!"); })
                .Subscribe(writingObserver);


            //using (var heatWriter = new JsonWriter(File.Open(args[0], FileMode.Create)))
            //    heatWriter.Write(events.ToEnumerable());

            Console.ReadLine();

        }
    }
}
