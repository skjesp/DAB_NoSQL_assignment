using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_NoSQL_assignment.Models {
    public class Circle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        [BsonElement]
        public string ForUser { get; set;} 

        [BsonElement]
        public string[] Members { get; set;} 
        
    }
}