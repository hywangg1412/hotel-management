using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Repositories.Base;

namespace FUMini.DataAccess.Repositories.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer GetCustomerByFullname(string fullname);
    }
}
