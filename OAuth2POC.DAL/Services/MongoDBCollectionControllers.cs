using MongoDB.Driver;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.DAL.Services
{
    public static class MongoDBCollectionControllers
    {
        public static IMongoCollection<UserInfo> UsersCollection
        {
            get { return MongoDBService.ConnectMongoDB.GetCollection<UserInfo>("Users"); }
        }

        public static IMongoCollection<TokenInfo> TokensCollection
        {
            get { return MongoDBService.ConnectMongoDB.GetCollection<TokenInfo>("Tokens"); }
        }
    }
}