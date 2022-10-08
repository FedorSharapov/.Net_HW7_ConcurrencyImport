using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Otus.Teaching.Concurrency.Import.Common.Dto
{
    [XmlRoot("Customers")]
    public class CustomersList
    {
        public List<Customer> Customers { get; set; }
    }
}