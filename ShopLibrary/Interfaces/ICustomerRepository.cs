using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Entities;

namespace ShopLibrary.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByName(string name);
        Task<Customer> GetCustomerById(string id);

        Task AddCustomer(string name);
        Task DeleteCustomerById(string Id);
        Task DeleteCustomerByName(string name);
        Task UpdateCustomer(Customer customer);

        
    }
}
