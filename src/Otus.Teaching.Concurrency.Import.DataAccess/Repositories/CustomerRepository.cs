using System;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private DatabaseContext _dataContext;

        public CustomerRepository(DatabaseContext dbContext)
        {
            _dataContext = dbContext;
        }

        public void AddCustomer(Customer customer)
        {
            //Add customer to data source
            _dataContext.Customers.Add(customer);
        }
    }
}