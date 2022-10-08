using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.DataAccess.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Common;
using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.DataAccess
{
    public static class ParserFactory
    {
        public static IDataParser<IEnumerable<Customer>> GetParser(string provider, string filePath)
        {
            if (provider.Equals(TypesFiles.Xml))
                return new XmlParser(filePath);
            else if (provider.Equals(TypesFiles.Csv))
                return new CsvParser(filePath);

            throw new System.Exception($"Invalid type parser \"{provider}\".");
        }
    }
}