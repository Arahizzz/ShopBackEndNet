using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(Mongo mongo)
        {
            _orders = mongo.GetCollection<Order>("orders");
        }

        public Task<List<Order>> GetOrders() => _orders.Find(new BsonDocument()).ToListAsync();

        public Task<Order> GetOrder(string id) => _orders.Find(order => order.Id == id).FirstAsync();

        public Task DeleteOrder(string id) => _orders.DeleteOneAsync(order => order.Id == id);

        public Task UpdateOrder(string id, Order order)
        {
            order.Id = id;
            return _orders.ReplaceOneAsync(item => item.Id == id, order);
        }

        public Task AddOrder(Order order) => _orders.InsertOneAsync(order);
    }
}
