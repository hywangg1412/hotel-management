using FUMini.BussinessObjects.Models;
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

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetByID(int id)
        {
            return _customerRepository.GetByID(id);
        }

        public void Update(Customer entity)
        {
            _customerRepository.Update(entity);
            _customerRepository.Save();
        }
    }
}
