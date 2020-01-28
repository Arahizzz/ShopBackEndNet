using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoAPI.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly IMongoCollection<Product> _products;

        public CategoryService(Mongo mongo)
        {
            _categories = mongo.GetCollection<Category>("categories");
            _products = mongo.GetCollection<Product>("products");
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _categories.Find(new BsonDocument()).ToListAsync();
            var tasks = new List<Task<Category>>();
            foreach (var category in categories)
                tasks.Add(FetchProducts(category));
            return await Task.WhenAll(tasks);
        }

        public async Task<Category> FetchProducts(Category category)
        {
            var products = await _products.Find<Product>(product => product.Categories.Contains(category.Id)).ToListAsync();
            category.Products = products;
            return category;
        }

        public async Task<Category?> FetchCategory(string id)
        {
            var products = await _products.Find<Product>(product => product.Categories.Contains(id)).ToListAsync();
            var category = await GetCategory(id);
            if (category != null)
                category.Products = products;
            return category;
        }

        public Task<Category> GetCategory(string id) => _categories.Find<Category>(cat => cat.Id == id).FirstAsync();

        public Task UpdateCategory(string id, Category category)
        {
            category.Id = id;
            return _categories.ReplaceOneAsync(item => item.Id == id, category);
        }

        public Task DeleteCategory(string id) => _categories.DeleteOneAsync(category => category.Id == id);

        public Task AddCategory(Category category) => _categories.InsertOneAsync(category);

    }
}
