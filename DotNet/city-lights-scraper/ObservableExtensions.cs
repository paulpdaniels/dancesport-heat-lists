using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DancingDuck
{
    public static class ObservableExtensions
    {

        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T, int> onNext)
        {
            int index = 0;
            return source.Do((n) => onNext(n, index++));
        }

        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T, int> onNext, Action onCompleted)
        {
            int index = 0;
            return source.Do((n) => onNext(n, index++), onCompleted);
        }

    }
}
