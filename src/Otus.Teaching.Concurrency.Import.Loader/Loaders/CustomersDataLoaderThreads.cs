using Otus.Teaching.Concurrency.Import.Common;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public class ParallelCustomersDataLoader : IDataLoader
    {
        private readonly IEnumerable<Customer> _customers;

        /// <summary>
        /// Показать сообщения об ошибке при инициализации
        /// </summary>
        public event Action<string> DisplayMessage;

        public ParallelCustomersDataLoader(IEnumerable<Customer> customers)
        {
            _customers = customers;
        }

        /// <summary>
        /// Параллельная загрузка клиентов в базу данных 
        /// </summary>
        public void LoadData()
        {
            var threads = new List<Thread>(AppSettings.NumThreads);

            var numCustomersPerThread = _customers.Count() / AppSettings.NumThreads;
            var remainderCustomers = _customers.Count() % AppSettings.NumThreads;

            for (int i = 0; i < AppSettings.NumThreads; i++)
            {
                var countCustomers = (i != AppSettings.NumThreads - 1) ? numCustomersPerThread : numCustomersPerThread + remainderCustomers;
                var customersPerThread = _customers.GetPart(i * numCustomersPerThread, countCustomers);

                var thread = new Thread(() =>
                {
                    using var dbContext = DatabaseContextFactory.CreateDbContext(AppSettings.TypeDb, AppSettings.DbConnectionString);
                    dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    var loader = new CustomersDataLoader(new CustomerRepository(dbContext), customersPerThread);
                    loader.DisplayMessage += DisplayMessage;
                    loader.LoadData();
                });
                thread.Name = $"PCDL_{i + 1}";

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
                thread.Join();
        }
    }
}