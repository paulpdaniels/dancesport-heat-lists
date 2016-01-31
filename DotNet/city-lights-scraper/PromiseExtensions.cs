using CsQuery.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck
{
    public static class PromiseExtensions
    {

        public static IObservable<T> ToObservable<T>(this IPromise<T> promise, IScheduler scheduler = null)
        {
            return new PromiseObservable<T>(promise, scheduler);
        }

    }


    internal class PromiseObservable<T> : ObservableBase<T>
    {
        private IPromise<T> promise;
        private IScheduler scheduler;

        public PromiseObservable(IPromise<T> promise, IScheduler scheduler)
        {
            this.promise = promise;
            this.scheduler = scheduler ?? Scheduler.Default;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer)
        {
            var disposable = new SerialDisposable();

            this.promise.Then(success =>
            {
                disposable.Disposable = this.scheduler.Schedule(success, (s, state) =>
                {
                    observer.OnNext(state);
                    observer.OnCompleted();
                    return Disposable.Empty;
                });
            });

            return disposable;
        }
    }
}
