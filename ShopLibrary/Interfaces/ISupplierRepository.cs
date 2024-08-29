using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Entities;

namespace ShopLibrary.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetSupplierByName(string supplierName);
        Task<Supplier> GetSupplierById(string supplierId);
        Task UpdateItemCount(string supplierID, string itemID, int count);
        Task AddSupplier(Supplier supplier);
        Task DeleteSupplier(string supplierId);
        Task<List<Supplier>> GetAllSuppliers();

        Task UpdateProfit(string supplierId, double profit);

    }
}
