using MongoDB.Bson;
using OAuth2POC.IDP.Services;
using OAuth2POC.Model.Enums;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OAuth2POC.XUnitTest.OAuth2POC.IDP.UnitTests.Services
{
    public class UserServiceUnitTest
    {
        [Fact]
        public void ShouldBeGetUsers()
        {
            UserInfo expected = new UserInfo()
            {
                UserId = ObjectId.Parse("5e5dd2e536eeff9b0efac5af"),
                FirstName = "test1",
                LastName = "test1",
                Username = "user1",
                Password = "pass1",
                Code = "Xwgp/I8CeOdCUXUsUQ2MWE7QV1JXqHxEK+4H5EeeT/nO0IG4scF0AQ==",
                CreateDate = Convert.ToDateTime("2020-03-03T03:45:41.056Z"),
                LastUpdateDate = Convert.ToDateTime("2020-03-18T07:15:05.715Z"),
                UserRole = UserRole.User
            };

            List<UserInfo> actual = new UserService().GetUsers();
            Assert.Contains(expected, actual);
        }

        [Fact]
        public void ShouldBeGetUser()
        {
            UserInfo expected = new UserInfo()
            {
                UserId = ObjectId.Parse("5e5dd2e536eeff9b0efac5af")
            };

            UserInfo actual = new UserService().GetUser("5e5dd2e536eeff9b0efac5af");
            Assert.Equal(expected.UserId, actual.UserId);
        }

        [Fact]
        public void ShouldBeInsertUser()
        {
            UserInfo userInfo = new UserInfo()
            {
                FirstName = "test20",
                LastName = "test20",
                Username = "user20",
                Password = "pass20",
                UserRole = UserRole.User
            };

            bool expected = true;
            bool actual = new UserService().InsertUser(userInfo);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldBeUpdateUser()
        {
            UserInfo userInfo = new UserInfo()
            {
                FirstName = "test30",
                LastName = "test30",
                Username = "user30",
                Password = "pass30",
                UserRole = UserRole.User
            };

            bool expected = true;
            bool actual = new UserService().UpdateUser(userInfo);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldBeDeleteUser()
        {
            bool expected = true;
            bool actual = new UserService().DeleteUser("5e6a0e277f1ac4e7cd437baf");
            Assert.Equal(expected, actual);
        }
    }
}