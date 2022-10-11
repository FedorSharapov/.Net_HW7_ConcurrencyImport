using Microsoft.Extensions.Logging;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;
using System;
using System.Collections.Generic;
using ILogger = Serilog.ILogger;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _dataContext;
        private readonly ILogger _logger;

        public CustomerRepository(DatabaseContext dbContext, ILogger logger)
        {
            _dataContext = dbContext;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.Warning (ex.Message);
                return false;
            }
        }
    }
}