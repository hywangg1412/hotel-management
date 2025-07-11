﻿using FUMini.BussinessObjects.Models;
using FUMini.Services.Base;

namespace FUMini.Services.Interfaces
{
    public interface ICustomerService : IBaseService<Customer, int>
    {
        bool IsEmailExist(string email);
        Customer FindCustomerByEmail(string email);
        void UpdateStatus(int customerId, byte status);
    }
}
