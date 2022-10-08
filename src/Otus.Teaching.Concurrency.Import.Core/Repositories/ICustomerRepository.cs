using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.Handler.Repositories
{
    public interface ICustomerRepository
    {
        void AddCustomers(IEnumerable<Customer> customers);

        bool SaveChanges();
    }
}