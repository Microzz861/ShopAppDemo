using ShopLibrary.Interfaces;
using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ShopLibrary.Repositories
{
    public class ItemRepository : DatabaseRepository, IItemRepository
    {
        public IMongoCollection<Item> ItemCollection { get; set; }

        public ItemRepository()
        {
            this.ItemCollection = MongoConnector<Item>("Items");
        }
        public async Task<List<Item>> GetAllItems()
        {
            var filter = Builders<Item>.Filter.Empty;
            var items = await ItemCollection.FindAsync(filter);
            return items.ToList();
        }

        public async Task<Item> GetById(string id)
        {
            var item = await(await ItemCollection.FindAsync(item => item.Id == id)).FirstOrDefaultAsync();
            return item;
        }

        public async Task<Item> GetByName(string name)
        {
            var filter = Builders<Item>.Filter.Eq("name", name);
            var item = await ItemCollection.FindAsync(filter);
            return item.FirstOrDefault();
        }

        public async Task UpdateItem(string itemId, int count, double profit)
        {
            var filter = Builders<Item>.Filter.Eq(item => item.Id, itemId);
            var update = Builders<Item>.Update.Set(item => item.Count, count).Set(item => item.Profit, profit);
            await ItemCollection.UpdateOneAsync(filter, update);
            
        }

        public async Task AddItem(Item item)
        {
            await ItemCollection.InsertOneAsync(item);
            Console.WriteLine("Successfully inserted item into item collection");
        }
        
    }
}
