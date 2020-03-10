using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using OAuth2POC.Model.JsonConverters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Models.Interface
{
    public class UserInfo
    {
        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string FirstName { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string LastName { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string Username { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string Password { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string Token { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreateDate { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdateDate { get; set; }
    }
}