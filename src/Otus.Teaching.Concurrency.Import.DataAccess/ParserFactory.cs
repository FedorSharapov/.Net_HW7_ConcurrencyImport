using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.DataAccess.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.DataAccess
{
    public static class ParserFactory
    {
        public static IDataParser<IEnumerable<Customer>> GetParser(string filePath)
        {
            return new XmlParser(filePath);
        }
    }
}