using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FastTechFoodsDLQProcessor.Models
{
    public class DLQMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
    }
}
