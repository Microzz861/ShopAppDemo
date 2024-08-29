using MongoDB.Driver;
using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace ShopLibrary.Repositories
{
    public class DatabaseRepository
    {
        private const string ConString = "mongodb://localhost:27017";
        private const string DBName = "Shop";
       
        public static IMongoCollection<T> MongoConnector<T>(in string collection)
        {
            var client = new MongoClient(ConString);
            var db = client.GetDatabase(DBName);
            return db.GetCollection<T>(collection);
        }
        
    }
}
