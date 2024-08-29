using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Entities;
using ShopLibrary.Interfaces;
using ShopLibrary.Repositories;

namespace ShopLibrary.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ItemRepository _itemRepository;
        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
            _itemRepository = new ItemRepository();
        }
        public async Task<double> PurchaseItemAsync(string customerName, string itemName, int quantity)
        {
            var item = await _itemRepository.GetByName(itemName);
            double itemProfit = item.Profit;
            var customer = await _customerRepository.GetCustomerByName(customerName);
            double budget = customer.Budget;
            int count = item.Count;
            double totalPrice = 0.0;
            double profit = await Task<double>.Run(() =>
            {
                if (budget >= totalPrice)
                {
                    totalPrice = item.Price * quantity;
                    budget = budget - totalPrice;
                    customer.Budget = budget;
                    Console.WriteLine($"{quantity} {item.Name} have been purchased and your current budget is {budget}");
                    count = count - quantity;
                    item.Count = count;
                    return totalPrice;
                }
                return totalPrice;
            });
            itemProfit = itemProfit + profit;
            await _customerRepository.UpdateCustomer(customer);
            await _itemRepository.UpdateItem(item.Id, count, itemProfit);
            return profit;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllCustomers();
            Console.WriteLine("Cool");
            return customers;
        }

        public async Task<Customer> GetCustomerByNameAsync(string name)
        {
            var customer = await _customerRepository.GetCustomerByName(name);
            return customer;
        }
        public async Task DeleteCustomerByNameAsync(string name)
        {
            await _customerRepository.DeleteCustomerByName(name);
        }
        public async Task AddCustomerAsync(string name)
        {
            var customer = await _customerRepository.GetCustomerByName(name);
            if(customer == null)
            {
                await _customerRepository.AddCustomer(name);
            }
            else
            {
                Console.WriteLine("Customer already exists within the database");
            }
        }

        public async Task DeleteCustomerByIdAsync(string Id)
        {
            await _customerRepository.DeleteCustomerById(Id);
        }
        public async Task<Customer> GetCustomerByIdAsync(string Id)
        {
            var customer = await _customerRepository.GetCustomerById(Id);
            return customer;
        }
    }
}
