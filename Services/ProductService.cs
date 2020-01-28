using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoAPI.Services
{
    public class ProductService
    {

        private readonly IMongoCollection<Product> _products;

        public ProductService(Mongo mongo)
        {
            _products = mongo.GetCollection<Product>("products");
        }

        public Task<List<Product>> GetProducts() => _products.Find(new BsonDocument()).ToListAsync();

        public Task<Product> GetProduct(string id) => _products.Find(product => product.Id == id).SingleOrDefaultAsync();

        public Task DeleteProduct(string id) => _products.DeleteOneAsync(product => product.Id == id);

        public Task UpdateProduct(string id, Product product)
        {
            product.Id = id;
            return _products.ReplaceOneAsync(item => item.Id == id, product);
        }

        public Task AddProduct(Product product) => _products.InsertOneAsync(product);
    }
}
