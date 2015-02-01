using CsQuery;
using CsQuery.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck
{
    public static class PromiseExtensions
    {

        public static IObservable<T> ToObservable<T>(this IPromise<T> promise)
        {
            var subject = new AsyncSubject<T>();

            promise.Then(new PromiseAction<T>((t) =>
            {
                subject.OnNext(t);
                subject.OnCompleted();
            }),
            new PromiseAction<T>(
            (t) =>
            {
                subject.OnError(new Exception());
            }));

            return subject.AsObservable();

        }

        public static IObservable<T> ToObservable<T>(this IPromise<T> promise, TimeSpan timeout)
        {
                        
            var subject = new AsyncSubject<T>();

            When.All((int)timeout.TotalMilliseconds, promise)
            .Then(new PromiseAction<T>((t) =>
            {
                subject.OnNext(t);
                subject.OnCompleted();
            }),
            new PromiseAction<T>(
            (t) =>
            {
                subject.OnError(new Exception());
            }));

            return subject.AsObservable();

        }

    }
}
