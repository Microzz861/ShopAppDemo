using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLibrary.Interfaces
{
    public interface IItemService
    {
        Task<double> BuyFromSupplierAsync(string supplierId, Item item, int quantity);
        Task<List<Item>> GetAllItemsAsync();
        Task<Item> GetByNameAsync(string name);

    }
}
