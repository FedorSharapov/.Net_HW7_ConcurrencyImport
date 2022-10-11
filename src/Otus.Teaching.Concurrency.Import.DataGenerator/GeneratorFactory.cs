using Otus.Teaching.Concurrency.Import.Common;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using Otus.Teaching.Concurrency.Import.Handler.Data;

namespace Otus.Teaching.Concurrency.Import.DataGenerator
{
    public static class GeneratorFactory
    {
        public static IDataGenerator GetGenerator(string provider)
        {
            if (provider.Equals(TypesFiles.Xml))
                return new XmlGenerator();
            else if (provider.Equals(TypesFiles.Csv))
                return new CsvGenerator();

            throw new System.Exception($"Invalid type generator \"{provider}\".");
        }
    }
}