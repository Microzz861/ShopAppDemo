using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopLibrary.Entities;
using ShopLibrary.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace ShopLibrary.Repositories
{
    public class SupplierRepository : DatabaseRepository, ISupplierRepository
    {
        public IMongoCollection<Supplier> SupplierCollection { get; set; }
        public SupplierRepository() {
            SupplierCollection = MongoConnector<Supplier>("Suppliers");
        }
        public async Task AddSupplier(Supplier supplier)
        {
           await SupplierCollection.InsertOneAsync(supplier);
            Console.WriteLine($"{supplier.Name} has been inserted into the database successfully");
        }

        public async Task<Supplier> GetSupplierByName(string supplierName)
        {
            var builder = Builders<Supplier>.Filter;
            var filter =  builder.Eq("name", supplierName);
            var supplier = await SupplierCollection.Find(filter).FirstOrDefaultAsync();
            return supplier;
        }

        
        public async Task UpdateItemCount(string supplierID, string itemID, int count)
        {
            
            var filter = Builders<Supplier>.Filter.Eq(s => s.Id, supplierID) & Builders<Supplier>.Filter.ElemMatch(s => s.items, Builders<Item>.Filter.Eq(s => s.Id, itemID));
            var update = Builders<Supplier>.Update.Set(sup => sup.items.FirstMatchingElement().Count, count); //s => s.items.FirstMatchingElement().Count
            await SupplierCollection.UpdateOneAsync(filter, update);            
        }
        public async Task UpdateSupplier(FilterDefinition<Supplier> filter, UpdateDefinition<Supplier> update)
        {

            await SupplierCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteSupplier(string supplierId)
        {
            var filter = Builders<Supplier>.Filter.Eq(s => s.Id, supplierId);
            await SupplierCollection.DeleteOneAsync(filter);
        }

        public async Task<Supplier> GetSupplierById(string supplierId)
        {
            var builder = Builders<Supplier>.Filter;
            var filter = builder.Eq(sup => sup.Id, supplierId);
            var supplier = await SupplierCollection.Find(filter).FirstOrDefaultAsync();
            return supplier;
        }

        public async Task<List<Supplier>> GetAllSuppliers()
        {
            var filter = Builders<Supplier>.Filter.Empty;
            var suppliers = await (await SupplierCollection.FindAsync(filter)).ToListAsync();
            return suppliers;
        }

        public async Task UpdateProfit(string supplierId, double profit)
        {
            var filter = Builders<Supplier>.Filter.Eq(sup => sup.Id, supplierId);
            var update = Builders<Supplier>.Update.Set(sup => sup.Profit, profit); 
            await SupplierCollection.UpdateOneAsync(filter, update);
        }
    }
}
