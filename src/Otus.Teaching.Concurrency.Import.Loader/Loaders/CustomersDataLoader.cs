using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;
using Otus.Teaching.Concurrency.Import.Common;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public class CustomersDataLoader : IDataLoader
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEnumerable<Customer> _customers;

        /// <summary>
        /// �������� ��������� �� ������ ��� �������������
        /// </summary>
        public event Action<string> DisplayMessage;

        public CustomersDataLoader(ICustomerRepository customerRepository, IEnumerable<Customer> customers)
        {
            _customerRepository = customerRepository;
            _customers = customers;
        }

        /// <summary>
        /// �������� �������� � ���� ������
        /// </summary>
        public void LoadData()
        {
            var numCustomersForSave = 200000;

            // ���������� �� 200000
            if (_customers.Count() < numCustomersForSave)
            {
                _customerRepository.AddCustomers(_customers);
                SaveChanges(_customers);
            }
            else
            {
                var numParts = 1 + _customers.Count() / numCustomersForSave;
                var customersParts = _customers.Chunks(numParts);

                foreach (var customers in customersParts)
                {
                    _customerRepository.AddCustomers(customers);
                    SaveChanges(customers);
                }
            }
        }

        /// <summary>
        /// ���������� ��������� � �� �� 10 ������� � ������ ������
        /// </summary>
        /// <param name="customers">��������� ��������</param>
        private void SaveChanges(IEnumerable<Customer> customers)
        {
            for (int a = 0; a < 10; a++)
            {
                if (_customerRepository.SaveChanges())
                {
                    if (a != 0)
                        DisplayMessage?.Invoke($"Attempt {a + 1}. Thread [\"{Thread.CurrentThread.ManagedThreadId}\"]. Customers from Id [{customers.First().Id}] to Id [{customers.Last().Id}] saved!");

                    return;
                }
                else
                    DisplayMessage?.Invoke($"Attempt {a + 1}. Thread [\"{Thread.CurrentThread.ManagedThreadId}\"]. Error save customers from Id [{customers.First().Id}] to Id [{customers.Last().Id}].");
            }
        }
    }
}