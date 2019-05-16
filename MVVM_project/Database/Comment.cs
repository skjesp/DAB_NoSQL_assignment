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

        // Indicates who owns the posts where the comment i connected to.
        [BsonElement]
        public string OwnerPostID { get; set; }

        [BsonElement]
        public string Writer_userID { get; set; }

        [BsonElement]
        public string Writer_userName { get; set; }


        //[BsonRepresentation(BsonType.Document)]
        //public User OwnerUser { get; set; }
    }
}
