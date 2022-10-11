using Otus.Teaching.Concurrency.Import.Common;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Loader;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public class CustomersDataLoaderThreads : IDataLoader
    {
        private readonly IEnumerable<Customer> _customers;

        /// <summary>
        /// Показать сообщения об ошибке при инициализации
        /// </summary>
        public event Action<string> DisplayMessage;

        public CustomersDataLoaderThreads(IEnumerable<Customer> customers)
        {
            _customers = customers;
        }

        /// <summary>
        /// Параллельная загрузка клиентов в базу данных 
        /// </summary>
        public void LoadData()
        {
            var countdownEvent = new CountdownEvent(AppSettings.NumThreads);

            var customersPerThread = _customers.Chunks(AppSettings.NumThreads);

            foreach (var customers in customersPerThread)
            {
                var action = new Action(() =>
                {
                    using var dbContext = DatabaseContextFactory.CreateDbContext(AppSettings.TypeDb, AppSettings.DbConnectionString);
                    dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    var loader = new CustomersDataLoader(new CustomerRepository(dbContext), customers);
                    loader.DisplayMessage += DisplayMessage;
                    loader.LoadData();
                    countdownEvent.Signal();
                });

                if (AppSettings.IsUseThreadPool)
                    ThreadPool.QueueUserWorkItem(start => action());
                else
                    new Thread(start => action()).Start();
            }

            countdownEvent.Wait();
            countdownEvent.Dispose();
        }
    }
}