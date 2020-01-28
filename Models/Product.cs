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
    public class ShortProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("old_price")]
        [JsonPropertyName("old_price")]
        public int? OldPrice { get; set; }

        [BsonElement("image_url")]
        [JsonPropertyName("image_url")]
        public string? ImgUrl { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Product : ShortProduct
    {
        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("categories")]
        public IEnumerable<string>? Categories { get; set; }
    }
}
