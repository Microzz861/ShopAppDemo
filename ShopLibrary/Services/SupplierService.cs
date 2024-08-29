using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Interfaces;
using ShopLibrary.Repositories;
using ShopLibrary.Entities;
using MongoDB.Driver;

namespace ShopLibrary.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly SupplierRepository _supplierRepository;
        private readonly ItemRepository _itemRepository;
        public SupplierService() {
            _supplierRepository = new SupplierRepository();
            _itemRepository = new ItemRepository();
        }

        public async Task AddSupplierAsync(Supplier supplier)
        { 
                var customer = await _supplierRepository.GetSupplierByName(supplier.Name);
            if (customer == null)
            {
                await _supplierRepository.AddSupplier(supplier);
            }
            else
            {
                Console.WriteLine("Customer already exists within the database");
            }  
        }

        public async Task DeleteSupplierByIdAsync(string supplierId)
        {
            await _supplierRepository.DeleteSupplier(supplierId);
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllSuppliers();
            return suppliers.ToList();
        }
        public async Task<Supplier> GetSupplierByNameAsync(string supplierName)
        {
            var supplier = await _supplierRepository.GetSupplierByName(supplierName);
            return supplier;
        }
    

        public async Task UpdateItemCountAsync(string supplierID, string itemID, int count)
        {
            await _supplierRepository.UpdateItemCount(supplierID, itemID, count);
        }
    }
}
