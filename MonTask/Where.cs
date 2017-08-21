using System;
using System.Threading.Tasks;
using Optional;

namespace MonTask {
    public static partial class Extensions {
        public static async Task<Option<TSource>>
            Where<TSource>(this Task<TSource> task, Func<TSource, bool> predicate) {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            var source = await task;
            return predicate(source)
                ? Option.Some(source)
                : Option.None<TSource>();
        }
    }
}