using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_NoSQL_assignment.Models {
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        [BsonElement]
        public string PostOwner { get; set; }

        [BsonElement]
        public string Text { get; set; }

        [BsonElement]
        public List<Comment> Comments { get; set; }

        [BsonElement]
        public Circle Circle { get; set; }
    }
}
