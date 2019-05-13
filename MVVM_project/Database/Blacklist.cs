using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_NoSQL_assignment.Models {
    public class Blacklist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        [BsonElement]
        public string ForUser { get; set;} 

        [BsonRepresentation(BsonType.Document)]
        public User User { get; set;} 
    }
}