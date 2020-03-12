using MongoDB.Bson;
using MongoDB.Driver;
using OAuth2POC.DAL.Services;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.DAL.Repositories.Repositories
{
    public class TokenRepository : Repositories<TokenInfo>
    {
        public TokenRepository()
        {

        }

        public override List<TokenInfo> GetAll<T>()
        {
            try
            {
                List<TokenInfo> listToken = MongoDBCollectionControllers.TokensCollection.Find(new BsonDocument()).ToList();
                return listToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override TokenInfo GetById<T>(string tokenId)
        {
            try
            {
                List<TokenInfo> listToken = MongoDBCollectionControllers.TokensCollection.Find(t => t.TokenId == ObjectId.Parse(tokenId)).ToList();
                return listToken.Find(t => t.TokenId == ObjectId.Parse(tokenId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<TokenInfo> GetByCriteria<T>(TokenInfo tokenInfo)
        {
            try
            {
                FilterDefinitionBuilder<TokenInfo> builder = new FilterDefinitionBuilder<TokenInfo>();
                FilterDefinition<TokenInfo> filter = !builder.Eq("TokenId", BsonNull.Value);

                if (!string.IsNullOrEmpty(tokenInfo.AccessToken))
                {
                    filter = filter & builder.Eq("AccessToken", tokenInfo.AccessToken);
                }

                if (tokenInfo.TokenType != TokenType.None)
                {
                    filter = filter & builder.Eq("TokenType", tokenInfo.TokenType);
                }

                if (!string.IsNullOrEmpty(tokenInfo.RefreshToken))
                {
                    filter = filter & builder.Eq("RefreshToken", tokenInfo.RefreshToken);
                }

                if (!string.IsNullOrEmpty(tokenInfo.ClientId))
                {
                    filter = filter & builder.Eq("ClientId", tokenInfo.ClientId);
                }

                List<TokenInfo> listToken = MongoDBCollectionControllers.TokensCollection.Find(filter).ToList();
                return listToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Insert<T>(TokenInfo tokenInfo)
        {
            try
            {
                TokenInfo tokenRequest = new TokenInfo()
                {
                    TokenId = ObjectId.GenerateNewId(),
                    AccessToken = tokenInfo.AccessToken,
                    TokenType = tokenInfo.TokenType,
                    ExpiresIn = tokenInfo.ExpiresIn,
                    RefreshToken = tokenInfo.RefreshToken,
                    ClientId = tokenInfo.ClientId,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = tokenInfo.ExpiresAt
                };

                MongoDBCollectionControllers.TokensCollection.InsertOne(tokenRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Update<T>(TokenInfo tokenInfo)
        {
            try
            {
                UpdateDefinition<TokenInfo> objToken = new UpdateDefinitionBuilder<TokenInfo>()
                    .Set(t => t.AccessToken, !string.IsNullOrEmpty(tokenInfo.AccessToken) ? tokenInfo.AccessToken : string.Empty)
                    .Set(t => t.TokenType, tokenInfo.TokenType != TokenType.None ? tokenInfo.TokenType : TokenType.None)
                    .Set(t => t.ExpiresIn, tokenInfo.ExpiresIn > 0 ? tokenInfo.ExpiresIn : 0)
                    .Set(t => t.RefreshToken, !string.IsNullOrEmpty(tokenInfo.RefreshToken) ? tokenInfo.RefreshToken : string.Empty)
                    .Set(t => t.ClientId, !string.IsNullOrEmpty(tokenInfo.ClientId) ? tokenInfo.ClientId : string.Empty)
                    .Set(t => t.CreatedAt, DateTime.UtcNow)
                    .Set(t => t.ExpiresAt, tokenInfo.ExpiresAt != null ? tokenInfo.ExpiresAt : Convert.ToDateTime(null));

                UpdateResult updateResult = MongoDBCollectionControllers.TokensCollection.UpdateOne(t => t.TokenId == tokenInfo.TokenId, objToken);

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

        public override bool Delete<T>(string tokenId)
        {
            try
            {
                DeleteResult deleteResult = MongoDBCollectionControllers.TokensCollection.DeleteOne(t => t.TokenId == ObjectId.Parse(tokenId));

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