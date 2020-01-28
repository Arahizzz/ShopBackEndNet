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
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [BindRequired]
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [BsonIgnore]
        [JsonPropertyName("products")]
        public IEnumerable<ShortProduct> Products { get; set; } = Enumerable.Empty<ShortProduct>();
    }

}
