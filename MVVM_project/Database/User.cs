using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_NoSQL_assignment.Models {
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        [BsonElement]
        public string Name { get; set;} 

        [BsonElement]
        public string Gender { get; set;}

        [BsonElement]
        public string Age { get; set;}

        [BsonElement]
        public string[] Circles { get; set; }

        [BsonElement]
        public string[] BlackList { get; set; }

        [BsonElement]
        public string[] FollowedUserIds { get; set; }
    }
}