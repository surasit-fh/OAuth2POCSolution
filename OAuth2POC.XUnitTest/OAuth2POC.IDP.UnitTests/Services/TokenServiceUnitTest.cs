using OAuth2POC.IDP.Services;
using OAuth2POC.Model.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OAuth2POC.XUnitTest.OAuth2POC.IDP.UnitTests.Services
{
    public class TokenServiceUnitTest
    {
        [Fact]
        public void ShouldBeGetToken()
        {
            string clientId = "5e6f069a0449decf8cab73f6";
            bool expected = true;
            TokenInfo actual = new TokenService().GetToken(clientId);
            Assert.Equal(expected, !string.IsNullOrEmpty(actual.AccessToken));
        }

        [Fact]
        public void ShouldBeRefreshToken()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJOV1UyWmpBMk9XRXdORFE1WkdWalpqaGpZV0kzTTJZMiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTU4NTAzMzM5MSwiZXhwIjoxNTg1MDM2OTkxLCJpYXQiOjE1ODUwMzMzOTEsImlzcyI6Ik9BdXRoMlBPQyIsImF1ZCI6IjVlNmYwNjlhMDQ0OWRlY2Y4Y2FiNzNmNiJ9.z4iWDN9peNlPlHwf_ObxcgFAG5iIYB9-t1ggJOCjx9E";
            string refreshToken = "iLHm8ahgyHJBNGeOZQWxUD1iX+GnYLcdd5IIGVnIHmY=";
            bool expected = true;
            TokenInfo actual = new TokenService().RefreshToken(token, refreshToken);
            Assert.Equal(expected, actual != null);
        }

        [Fact]
        public void ShouldBeValidateToken()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJOV1UyWmpBMk9XRXdORFE1WkdWalpqaGpZV0kzTTJZMiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTU4NTAzMzM5MSwiZXhwIjoxNTg1MDM2OTkxLCJpYXQiOjE1ODUwMzMzOTEsImlzcyI6Ik9BdXRoMlBPQyIsImF1ZCI6IjVlNmYwNjlhMDQ0OWRlY2Y4Y2FiNzNmNiJ9.z4iWDN9peNlPlHwf_ObxcgFAG5iIYB9-t1ggJOCjx9E";
            bool expected = true;
            bool actual = new TokenService().ValidateToken(token);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldBeRevokeToken()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJOV1UyWmpBMk9XRXdORFE1WkdWalpqaGpZV0kzTTJZMiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTU4NTAzMzM5MSwiZXhwIjoxNTg1MDM2OTkxLCJpYXQiOjE1ODUwMzMzOTEsImlzcyI6Ik9BdXRoMlBPQyIsImF1ZCI6IjVlNmYwNjlhMDQ0OWRlY2Y4Y2FiNzNmNiJ9.z4iWDN9peNlPlHwf_ObxcgFAG5iIYB9-t1ggJOCjx9E";
            bool expected = true;
            bool actual = new TokenService().RevokeToken(token);
            Assert.Equal(expected, actual);
        }
    }
}