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
        //[JsonProperty(PropertyName = "id_token")]
        [BsonIgnoreIfNull]
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId TokenId { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "access_token")]
        [BsonIgnoreIfNull]
        public string AccessToken { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "token_type")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.String)]
        public TokenType TokenType { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "expires_in")]
        [BsonIgnoreIfNull]
        public int ExpiresIn { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "refresh_token")]
        [BsonIgnoreIfNull]
        public string RefreshToken { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "client_id")]
        [BsonIgnoreIfNull]
        //[BsonElement("_id")]
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //[JsonConverter(typeof(ObjectIdConverter))]
        public string ClientId { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "created_at")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "expires_at")]
        [BsonIgnoreIfNull]
        public DateTime ExpiresAt { get; set; }
    }
}