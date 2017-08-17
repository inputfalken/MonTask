using System;
using System.Threading.Tasks;

namespace MonTask {
    public static partial class Extensions {
        public static async Task<TSource> Where<TSource>(this Task<TSource> task, Func<TSource, bool> predicate) {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            var source = await task;
            // TODO Install Option<T>
            return predicate(source) ? source : default(TSource);
        }
    }
}