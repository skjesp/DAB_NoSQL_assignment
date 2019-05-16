using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_NoSQL_assignment.Models {
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        [BsonElement]
        public string Text { get; set;}

        [BsonElement]
        public string User { get; set;} 
    }
}