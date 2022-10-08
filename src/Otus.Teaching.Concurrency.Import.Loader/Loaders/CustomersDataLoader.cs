using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public class CustomersDataLoader : IDataLoader
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEnumerable<Customer> _customers;

        /// <summary>
        /// Показать сообщения об ошибке при инициализации
        /// </summary>
        public event Action<string> DisplayMessage;

        public CustomersDataLoader(ICustomerRepository customerRepository, IEnumerable<Customer> customers)
        {
            _customerRepository = customerRepository;
            _customers = customers;
        }

        /// <summary>
        /// Загрузка клиентов в базу данных
        /// </summary>
        public void LoadData()
        {
            var countCustomerForSave = 100000;

            // сохранение по 100000
            if (_customers.Count() < countCustomerForSave)
            {
                _customerRepository.AddCustomers(_customers);
                SaveChanges(_customers);
            }
            else
            {
                var numParts = _customers.Count() / countCustomerForSave;
                var remainderCustomers = _customers.Count() % numParts;

                for (int i = 0; i < numParts; i++)
                {
                    var customersPart = _customers
                                                .Skip(i * countCustomerForSave)
                                                .Take((i != numParts - 1) ?
                                                            countCustomerForSave :
                                                            countCustomerForSave + remainderCustomers);

                    _customerRepository.AddCustomers(customersPart);
                    SaveChanges(customersPart);
                }
            }
        }

        /// <summary>
        /// Сохранение изменений в БД за 10 попыток случае ошибок
        /// </summary>
        /// <param name="customers">коллекция клиентов</param>
        private void SaveChanges(IEnumerable<Customer> customers)
        {
            for (int a = 0; a < 10; a++)
            {
                if (_customerRepository.SaveChanges())
                {
                    if (a != 0)
                        DisplayMessage?.Invoke($"Attempt {a + 1}. Thread [\"{Thread.CurrentThread.Name}\"]. Customers from Id [{customers.First().Id}] to Id [{customers.Last().Id}] saved!");

                    return;
                }
                else
                    DisplayMessage?.Invoke($"Attempt {a + 1}. Thread [\"{Thread.CurrentThread.Name}\"]. Error save customers from Id [{customers.First().Id}] to Id [{customers.Last().Id}].");
            }
        }
    }
}