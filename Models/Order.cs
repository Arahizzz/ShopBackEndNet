using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemoAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("phone")]
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("address")]
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("products")]
        [JsonPropertyName("products")]
        public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
    }

    [BsonIgnoreExtraElements]
    public class Item
    {
        [BsonId] [JsonIgnore] public ObjectId ObjectId { get; set; }

        [BindRequired]
        [BsonElement("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 0;
    }
}