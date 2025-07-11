﻿using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Interfaces;

namespace FUMini.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Add(Customer entity)
        {
            _customerRepository.Insert(entity);
            _customerRepository.Save();
        }

        public void Delete(int id)
        {
            _customerRepository.Delete(id);
            _customerRepository.Save();
        }

        public void Delete(Customer entity)
        {
            _customerRepository.Delete(entity);
            _customerRepository.Save();
        }

        public Customer FindCustomerByEmail(string email)
        {
            return _customerRepository.GetAll().FirstOrDefault(c => c.EmailAddress == email);
        }

        public List<Customer> GetAll()
        {
            return _customerRepository.GetAll().ToList();
        }

        public Customer GetByID(int id)
        {
            return _customerRepository.GetByID(id);
        }

        public bool IsEmailExist(string email)
        {
            return _customerRepository.GetAll().Any(c => c.EmailAddress == email);
        }

        public void Update(Customer entity)
        {
            _customerRepository.Update(entity);
            _customerRepository.Save();
        }

        public void UpdateStatus(int customerId, byte status)
        {
            var customer = GetByID(customerId);
            if (customer != null)
            {
                customer.CustomerStatus = status;
                Update(customer);
            }
        }
    }
}
