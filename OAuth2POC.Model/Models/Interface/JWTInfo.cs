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
    public class JWTInfo
    {
        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "payload")]
        public Payload Payload { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }
    }

    public class Header
    {
        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "typ")]
        public string Type { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "alg")]
        public string Algorithm { get; set; }
    }

    public class Payload
    {
        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "jti")]
        [BsonIgnoreIfNull]
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId JWTid { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "iss")]
        public string Issuer { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "sub")]
        public string Subject { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "aud")]
        [BsonIgnoreIfNull]
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Audience { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "exp")]
        public int ExpirationTime { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "nbf")]
        public int NotBefore { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "iat")]
        public int IssuedAt { get; set; }
    }
}