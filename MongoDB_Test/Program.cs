using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MongoDB_Test
{
    //https://www.codeproject.com/Articles/1135453/%2FArticles%2F1135453%2FMongoDB-CRUD-Operations-With-Net
    class Program
    {
        static void Main(string[] args)
        {
            Execute_();
        }

        public static void Execute_()
        {
            
            var p = System.Web.HttpUtility.UrlEncode("1234");
            var conn = "mongodb://jbloo5:" + p + "@jontest-shard-00-00-v3de0.mongodb.net:27017,jontest-shard-00-01-v3de0.mongodb.net:27017,jontest-shard-00-02-v3de0.mongodb.net:27017/test?ssl=true&replicaSet=JonTest-shard-0&authSource=admin";
            //var client = new MongoClient("mongodb://jbloo5:<PASSWORD>@jontest-shard-00-00-v3de0.mongodb.net:27017,jontest-shard-00-01-v3de0.mongodb.net:27017,jontest-shard-00-02-v3de0.mongodb.net:27017/test?ssl=true&replicaSet=JonTest-shard-0&authSource=admin");
            //var c = "mongodb+srv://jbloo5:" + p + "@jontest-v3de0.mongodb.net/test?retryWrites=true";

            var client = new MongoClient(conn);
            var session = client.StartSession();
            var database = session.Client.GetDatabase("TradeSimulations");
            var collection = database.GetCollection<Product>("TradeSimulations");

            collection.Database.DropCollection("TradeSimulations");

            List<Product> list = new List<Product>();
            list.Add(new Product { Ticker = "VOD LN", Name = "Vodafone", Price = 150 });
            list.Add(new Product { Ticker = "BT LN", Name = "BT", Price = 300 });
            
            session.StartTransaction();
            collection.InsertManyAsync(list);

            var filter = new BsonDocument("Name", "Vodafone");
            var results = collection.Find<Product>(filter).ToList();

            foreach (Product item in results)
                Console.WriteLine(item.Price.ToString());
        }

        public class Product
        {
            [BsonId]
            public ObjectId Id { get; set; }
            [BsonElement("Ticker")]
            public string Ticker { get; set; }
            [BsonElement("Name")]
            public string Name { get; set; }
            [BsonElement("Price")]
            public Double Price { get; set; }
        }
    }
}
