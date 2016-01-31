using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck
{
    public static class EnumerableExtensions
    {

        public static IEnumerable<TOut> Zip<T1, T2, T3, T4, TOut>(
            this IEnumerable<T1> source1,
            IEnumerable<T2> source2,
            IEnumerable<T3> source3,
            IEnumerable<T4> source4,
            Func<T1, T2, T3, T4, TOut> selector
            )
        {
            var e1 = source1.GetEnumerator();
            var e2 = source2.GetEnumerator();
            var e3 = source3.GetEnumerator();
            var e4 = source4.GetEnumerator();

            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
            {
                yield return selector(e1.Current, e2.Current, e3.Current, e4.Current);
            }
        }

    }
}
