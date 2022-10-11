using System.Collections.Generic;
using System.Linq;

namespace Otus.Teaching.Concurrency.Import.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> source, int numOfChunk)
        {
            var numCustomersPerThread = 1 + source.Count() / numOfChunk;
            var remainderCustomers = source.Count() % numOfChunk;
        
            for (int i = 0; i < numOfChunk; i++)
            {
                var countCustomers = (i != numOfChunk - 1) ? numCustomersPerThread : numCustomersPerThread + remainderCustomers;
                yield return source.Skip(i* numCustomersPerThread).Take(countCustomers);
            }
        }
    }
}
