using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ShopLibrary.Entities
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("profit")]

        public double Profit { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }
       

        public Item() {}
        public Item(string name, int count, double price, string category, double profit) { 
            this.Name = name;
            this.Count = count;
            this.Price = price;
            this.Category = category;
            this.Profit = profit;
        }


    }

}
