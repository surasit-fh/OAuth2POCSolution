using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.Model.JsonConverters
{
    public class ObjectIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectId);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType != JsonToken.String)
                    throw new Exception($"Unexpected token parsing ObjectId. Expected String, got {reader.TokenType}.");

                string value = (string)reader.Value;
                return string.IsNullOrEmpty(value) ? ObjectId.Empty : ObjectId.Parse(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                ObjectId parseValue = ObjectId.Parse(value.ToString());
                if (parseValue != null)
                {
                    if (value.GetType() == typeof(ObjectId))
                    {
                        ObjectId objectId = (ObjectId)value;
                        writer.WriteValue(objectId != ObjectId.Empty ? objectId.ToString() : string.Empty);
                    }
                    else
                    {
                        ObjectId objectId = (ObjectId)parseValue;
                        writer.WriteValue(objectId != ObjectId.Empty ? objectId.ToString() : string.Empty);
                    }
                }
                else
                {
                    throw new Exception("Expected ObjectId value.");
                }
            }
            catch (Exception)
            {
                throw new Exception("Expected ObjectId value.");
            }
        }
    }
}