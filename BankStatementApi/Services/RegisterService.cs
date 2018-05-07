using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BudgetHelperAPI.Services
{
    public class RegisterService
    {
        public AmazonCognitoIdentityProviderClient client { get; private set; }

        public RegisterService(AmazonCognitoIdentityProviderClient Client)
        {
            client = Client;
        }

        public SignUpResponse Register(SignUpRequest request)
        {
              return client.SignUp(request);
        }

        public SignUpRequest BuildSignUpRequest(string username, string password, Dictionary<string, string> attributes)
        {
            return new SignUpRequest()
            {
                ClientId = ConfigurationManager.AppSettings["CLIENT_ID"],
                Username = username,
                Password = password,
                UserAttributes = BuildAttributesList(attributes)
            };
        }

        private List<AttributeType> BuildAttributesList(Dictionary<string, string> attributes)
        {
            var attributeList = new List<AttributeType>();

            foreach (var attribute in attributes)
            {
                attributeList.Add(new AttributeType()
                {
                    Name = attribute.Key,
                    Value = attribute.Value
                });
            }

            return attributeList;
        }
    }
}