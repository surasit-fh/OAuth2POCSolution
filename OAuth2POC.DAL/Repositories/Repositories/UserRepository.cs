using MongoDB.Bson;
using MongoDB.Driver;
using OAuth2POC.DAL.Services;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.DAL.Repositories.Repositories
{
    public class UserRepository : Repositories<UserInfo>
    {
        public UserRepository()
        {

        }

        public override List<UserInfo> GetAll<T>()
        {
            try
            {
                List<UserInfo> listUser = MongoDBCollectionControllers.UsersCollection.Find(new BsonDocument()).ToList();
                return listUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override UserInfo GetById<T>(string userId)
        {
            try
            {
                List<UserInfo> listUser = MongoDBCollectionControllers.UsersCollection.Find(u => u.UserId == ObjectId.Parse(userId)).ToList();
                UserInfo user = listUser.Find(u => u.UserId == ObjectId.Parse(userId));
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<UserInfo> GetByCriteria<T>(UserInfo userInfo)
        {
            try
            {
                FilterDefinitionBuilder<UserInfo> builder = new FilterDefinitionBuilder<UserInfo>();
                FilterDefinition<UserInfo> filter = !builder.Eq("UserId", BsonNull.Value);

                if (!string.IsNullOrEmpty(userInfo.FirstName))
                {
                    filter = filter & builder.Eq("FirstName", userInfo.FirstName);
                }

                if (!string.IsNullOrEmpty(userInfo.LastName))
                {
                    filter = filter & builder.Eq("LastName", userInfo.LastName);
                }

                if (!string.IsNullOrEmpty(userInfo.Username))
                {
                    filter = filter & builder.Eq("Username", userInfo.Username);
                }

                if (!string.IsNullOrEmpty(userInfo.Token))
                {
                    filter = filter & builder.Eq("Token", userInfo.Token);
                }

                List<UserInfo> listUser = MongoDBCollectionControllers.UsersCollection.Find(filter).ToList();
                return listUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Insert<T>(UserInfo userInfo)
        {
            try
            {
                UserInfo userRequest = new UserInfo()
                {
                    UserId = ObjectId.GenerateNewId(),
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Username = userInfo.Username,
                    Password = userInfo.Password,
                    Token = userInfo.Token,
                    CreateDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now
                };

                MongoDBCollectionControllers.UsersCollection.InsertOne(userRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Update<T>(UserInfo userInfo)
        {
            try
            {
                UpdateDefinition<UserInfo> objUser = new UpdateDefinitionBuilder<UserInfo>()
                    .Set(u => u.FirstName, !string.IsNullOrEmpty(userInfo.FirstName) ? userInfo.FirstName : string.Empty)
                    .Set(u => u.LastName, !string.IsNullOrEmpty(userInfo.LastName) ? userInfo.LastName : string.Empty)
                    .Set(u => u.Username, !string.IsNullOrEmpty(userInfo.Username) ? userInfo.Username : string.Empty)
                    .Set(u => u.Password, !string.IsNullOrEmpty(userInfo.Password) ? userInfo.Password : string.Empty)
                    .Set(u => u.Token, !string.IsNullOrEmpty(userInfo.Token) ? userInfo.Token : string.Empty)
                    .Set(u => u.LastUpdateDate, DateTime.Now);

                UpdateResult updateResult = MongoDBCollectionControllers.UsersCollection.UpdateOne(u => u.UserId == userInfo.UserId, objUser);

                if (updateResult.IsAcknowledged)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Delete<T>(string userId)
        {
            try
            {
                DeleteResult deleteResult = MongoDBCollectionControllers.UsersCollection.DeleteOne(u => u.UserId == ObjectId.Parse(userId));

                if (deleteResult.IsAcknowledged)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}