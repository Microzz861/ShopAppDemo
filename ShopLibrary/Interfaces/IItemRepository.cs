using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLibrary.Interfaces
{
    public interface IItemRepository
    {
        
        Task<List<Item>> GetAllItems();
        Task<Item> GetById(string id);

        Task<Item> GetByName(string name);

        Task UpdateItem(string itemId, int count, double profit);
        

    }
}
