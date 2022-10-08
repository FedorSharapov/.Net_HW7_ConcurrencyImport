using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using Otus.Teaching.Concurrency.Import.Handler.Data;

namespace Otus.Teaching.Concurrency.Import.DataGenerator
{
    public static class GeneratorFactory
    {
        public static IDataGenerator GetGenerator(string fileName, int dataCount)
        {
            return new XmlGenerator(fileName, dataCount);
        }
    }
}