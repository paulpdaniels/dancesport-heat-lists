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

        public static System.Runtime.CompilerServices.TaskAwaiter<T> GetAwaiter<T>(this IPromise<T> promise)
        {
            TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
            promise.Then(new PromiseAction<T>((t) => completionSource.SetResult(t)), new PromiseAction<T>((t) => completionSource.SetException(new Exception())));
            return completionSource.Task.GetAwaiter();
        }

    }
}
