using System;
using System.Threading.Tasks;

namespace MonTask {
    public static partial class Extensions {
        public static async Task<TResult> Select<TSource, TResult>(this Task<TSource> task,
            Func<TSource, TResult> selector) {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return selector(await task.ConfigureAwait(false));
        }

        public static async Task<TResult> Select<TResult>(this Task task,
            Func<TResult> selector) {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            await task.ConfigureAwait(false);
            return selector();
        }
    }
}