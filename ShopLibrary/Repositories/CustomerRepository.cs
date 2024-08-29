using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Entities;
using ShopLibrary.Interfaces;
using System.Runtime.CompilerServices;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ShopLibrary.Repositories
{
    public class CustomerRepository : DatabaseRepository,ICustomerRepository
    {
        public IMongoCollection<Customer> CustomerCollection { get; set; }
        public CustomerRepository()
        {
            this.CustomerCollection = MongoConnector<Customer>("Customers");
        }
        public async Task DeleteCustomerById(string Id)
        {
            await CustomerCollection.DeleteOneAsync(c => c.Id == Id);
        }
        public async Task DeleteCustomerByName(string name)
        {
            await CustomerCollection.DeleteOneAsync(c => c.Name == name);
        }
        public async Task<Customer> GetCustomerByName(string name)
        {
            try
            {
                var customerCursor = await CustomerCollection.FindAsync(c => c.Name == name);
                var customer = await customerCursor.SingleOrDefaultAsync();
                return customer;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                throw;
            }         
        }
        public async Task AddCustomer(string name)
        {
            var customer = new Customer() { Name = name,Budget = 50.0 };
            await CustomerCollection.InsertOneAsync(customer);
            Console.WriteLine($"{customer.Name} has been successfully added into the database!");
        }
        public async Task<Customer> GetCustomerById(string id)
        {
            var customer = await CustomerCollection.FindAsync(c => c.Id == id);
            return customer.First();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var filter = Builders<Customer>.Filter.Empty;
            var customers =  await CustomerCollection.FindAsync(filter);
            return customers.ToEnumerable();
        }
        public void printInfo(Customer cus)
        {
            Console.WriteLine($"Customer Name: {cus.Name}, Budget: {cus.Budget}");
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, customer.Id);
            await CustomerCollection.FindOneAndReplaceAsync(filter, customer);
            

        }

    }
}
