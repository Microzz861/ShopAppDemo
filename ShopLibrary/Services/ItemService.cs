using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Interfaces;
using ShopLibrary.Repositories;
using ShopLibrary.Entities;
using System.Drawing;
using MongoDB.Driver.Core.Authentication;

namespace ShopLibrary.Services
{
    public class ItemService : IItemService
    {
        private readonly ItemRepository _itemRepository;
        private readonly SupplierRepository _supplierRepository;
        public ItemService()
        {
            _itemRepository = new ItemRepository();
            _supplierRepository = new SupplierRepository();
        }

        //Item object is passed, quantity or amount of items to be purchased is passed in integer, and finally the supplierId is passed in string.
        // BuyFromSupplier uses the item to determine its price, id, and count. If 
        //
        public async Task<double> BuyFromSupplierAsync(string supplierId, Item item, int quantity)
        {
            int count = item.Count;
            var itemId = item.Id;
            var supplier = await _supplierRepository.GetSupplierById(supplierId);
            double supplierProfit = supplier.Profit;
            var shopItem = await _itemRepository.GetByName(item.Name);
            if (shopItem == null)
            {
                await _itemRepository.AddItem(new Item { Name = item.Name, Category = item.Category, Price = (item.Price + 1), Count = quantity });
                supplierProfit = supplierProfit + (item.Price * quantity);
                await _supplierRepository.UpdateItemCount(supplierId, itemId, count - quantity);
                await _supplierRepository.UpdateProfit(supplierId, supplierProfit);
                return (item.Price * quantity);
            }
            else
            { 
                double itemProfit = item.Profit;
                double totalPrice = await Task<double>.Run(() =>
                {
                    double totalPrice = item.Price * quantity;
                    count = count - quantity;
                    item.Count = count;
                    return totalPrice;
                    //Insert render function again

                });

                
                supplierProfit = supplierProfit + totalPrice;
                string shopItemId = shopItem.Id;
                int shopItemCount = shopItem.Count + quantity;
                itemProfit = itemProfit + -totalPrice;
                await _supplierRepository.UpdateItemCount(supplierId, itemId, count);
                await _supplierRepository.UpdateProfit(supplierId, supplierProfit);
                await _itemRepository.UpdateItem(shopItemId, shopItemCount, itemProfit);
                return -totalPrice;
            }
        }
        public Task<List<Item>> GetAllItemsAsync()
        {
            return _itemRepository.GetAllItems();
        }

        public async Task<Item> GetByNameAsync(string name) { 
            var item = await _itemRepository.GetByName(name);
            return item;
        }

        
    }
}


