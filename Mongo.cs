using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoAPI
{
    public class Mongo
    {
        private readonly IMongoDatabase _db;

        public Mongo(IDataBaseSettings settings)
        {
            _db = new MongoClient(settings.ConnectionString).GetDatabase(settings.Name);
        }

        public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);
    }
}