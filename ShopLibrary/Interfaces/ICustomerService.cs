using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLibrary.Interfaces
{
    public interface ICustomerService
    {
        Task<double> PurchaseItemAsync(string customerName, string itemName, int quantity);
        Task DeleteCustomerByIdAsync(string Id);
        Task DeleteCustomerByNameAsync(string Name);
        Task AddCustomerAsync(string name);
        Task<Customer> GetCustomerByNameAsync(string name);

        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(string Id);
      
    }
}
