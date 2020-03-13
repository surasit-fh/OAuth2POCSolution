using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.JsonConverters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OAuth2POC.Model.Models.Interface
{
    public class TokenInfo
    {
        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId TokenId { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string AccessToken { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.String)]
        public TokenType TokenType { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public int ExpiresIn { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public string RefreshToken { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        //[BsonElement("_id")]
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? ClientId { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public DateTime ExpiresAt { get; set; }

        [DefaultValue(null)]
        [BsonIgnoreIfNull]
        public TokenStatus TokenStatus { get; set; }
    }
}