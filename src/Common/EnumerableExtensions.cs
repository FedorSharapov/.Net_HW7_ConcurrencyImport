using System.Collections.Generic;
using System.Linq;

namespace Otus.Teaching.Concurrency.Import.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> GetPart<T>(this IEnumerable<T> source, int skip, int take)
        {
            return source.Skip(skip).Take(take);
        }
    }
}
