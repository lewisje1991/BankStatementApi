using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BudgetHelperAPI.DTOs;
using System.Collections.Generic;
using System.Configuration;

namespace BudgetHelperAPI.Services
{
    public class LoginService
    {
        public AmazonCognitoIdentityProviderClient client { get; private set; }

        public LoginService(AmazonCognitoIdentityProviderClient Client)
        {
            client = Client;
        }

        public AdminInitiateAuthResponse Login(LoginRequestDTO request)
        {
            var authParams = new Dictionary<string, string>();
            authParams.Add("USERNAME", request.username);
            authParams.Add("PASSWORD", request.password);


            var authRequest = new AdminInitiateAuthRequest() {
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                AuthParameters = authParams,
                ClientId = ConfigurationManager.AppSettings["CLIENT_ID"],
                UserPoolId = ConfigurationManager.AppSettings["USERPOOL_ID"]
            };

            return client.AdminInitiateAuth(authRequest);
        }
    }
}