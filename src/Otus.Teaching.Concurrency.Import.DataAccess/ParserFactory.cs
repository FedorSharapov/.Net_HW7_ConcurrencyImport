using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.DataAccess.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Common;
using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.DataAccess
{
    public static class ParserFactory
    {
        public static IDataParser<IEnumerable<Customer>> GetParser(string provider)
        {
            if (provider.Equals(TypesFiles.Xml))
                return new XmlParser();
            else if (provider.Equals(TypesFiles.Csv))
                return new CsvParser();

            throw new System.Exception($"Invalid type parser \"{provider}\".");
        }
    }
}