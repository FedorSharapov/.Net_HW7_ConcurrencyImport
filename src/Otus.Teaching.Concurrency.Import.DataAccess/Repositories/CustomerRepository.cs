using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;
using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _dataContext;

        public CustomerRepository(DatabaseContext dbContext)
        {
            _dataContext = dbContext;
        }

        public void AddCustomers(IEnumerable<Customer> customers)
        {
            _dataContext.Customers.AddRange(customers);
        }

        public bool SaveChanges()
        {
            try
            {
                _dataContext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}