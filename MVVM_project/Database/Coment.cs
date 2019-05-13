using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_NoSQL_assignment.Models {
    public class Coment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        [BsonElement]
        public string Text { get; set;}

        [BsonRepresentation(BsonType.Document)]
        public User User { get; set;} 
    }
}