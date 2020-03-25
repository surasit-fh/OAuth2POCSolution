using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.DAL.Services
{
    internal class MongoDBService
    {
        private static string connectionString = "mongodb://admin:OAuth2POC@192.168.2.106:27017/OAuth2POC";

        internal static IMongoDatabase ConnectMongoDB
        {
            get
            {
                MongoUrl _database = MongoUrl.Create(connectionString);
                MongoClientSettings settings = new MongoClientSettings()
                {
                    Server = _database.Server,
                    ServerSelectionTimeout = _database.ServerSelectionTimeout,
                    SocketTimeout = _database.SocketTimeout,
                    Credential = MongoCredential.CreateCredential(_database.DatabaseName, _database.Username, _database.Password)
                };

                MongoClient client = new MongoClient(settings);
                return client.GetDatabase(_database.DatabaseName);
            }
        }
    }
}