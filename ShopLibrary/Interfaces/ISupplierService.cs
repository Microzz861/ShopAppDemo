using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLibrary.Interfaces
{
    public interface ISupplierService
    { 
        
        Task<Supplier> GetSupplierByNameAsync(string supplierName);
        Task UpdateItemCountAsync(string supplierID, string itemID, int count);
        Task AddSupplierAsync(Supplier supplier);
        Task DeleteSupplierByIdAsync(string supplierId);
        Task<List<Supplier>> GetAllSuppliersAsync();
    }
}
