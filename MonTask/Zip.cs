using System;
using System.Threading.Tasks;

namespace MonTask {
    public static partial class Extensions {
        public static async Task<TResult> Zip<TFirst, TSecond, TResult>(this Task<TFirst> first, Task<TSecond> second,
            Func<TFirst, TSecond, TResult> selector) {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return selector(await first.ConfigureAwait(false), await second.ConfigureAwait(false));
        }
    }
}