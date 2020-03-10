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
    public class AuthenticationInfo
    {
        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "client_id")]
        [BsonIgnoreIfNull]
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId ClientId { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "client_secret")]
        [BsonIgnoreIfNull]
        public string ClientSecret { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "grant_type")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.String)]
        public GrantType GrantType { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "code")]
        [BsonIgnoreIfNull]
        public string Code { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "state")]
        [BsonIgnoreIfNull]
        public string State { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "scope")]
        [BsonIgnoreIfNull]
        public string[] Scope { get; set; }

        [DefaultValue(null)]
        //[JsonProperty(PropertyName = "redirect_uri")]
        [BsonIgnoreIfNull]
        public string RedirectURI { get; set; }
    }
}