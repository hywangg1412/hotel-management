using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Base;
using FUMini.DataAccess.Repositories.Interfaces;

namespace FUMini.DataAccess.Repositories.Implementation
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly FUMiniContext _context;

        public CustomerRepository(FUMiniContext context) : base(context)
        {
            _context = context;
        }

        public Customer GetCustomerByFullname(string fullname)
        {
            var user = GetAll(u => u.CustomerFullName == fullname).FirstOrDefault();
            return user;
        }
    }
}
