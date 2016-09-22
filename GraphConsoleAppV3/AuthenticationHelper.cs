using System;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GraphConsoleAppV3
{
    internal class AuthenticationHelper
    {
        public static string TokenForUser;

        /// <summary>
        /// Get Token for Application.
        /// </summary>
        /// <returns>Token for application.</returns>
        public static async Task<string> GetTokenForApplicationAsync()
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(Constants.AuthString, false);
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(Constants.ClientId, Constants.ClientSecret);
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(Constants.ResourceUrl,
                clientCred);
            string token = authenticationResult.AccessToken;
            return token;
        }

        /// <summary>
        /// Get Active Directory Client for Application.
        /// </summary>
        /// <returns>ActiveDirectoryClient for Application.</returns>
        public static ActiveDirectoryClient GetActiveDirectoryClientAsApplication()
        {
            Uri servicePointUri = new Uri(Constants.ResourceUrl);
            Uri serviceRoot = new Uri(servicePointUri, Constants.TenantId);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot, GetTokenForApplicationAsync);
            return activeDirectoryClient;
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public static async Task<string> GetTokenForUserAsync()
        {
            if (TokenForUser == null)
            {
                var redirectUri = new Uri("https://localhost");
                AuthenticationContext authenticationContext = new AuthenticationContext(Constants.AuthString, false);
                AuthenticationResult userAuthnResult = await authenticationContext.AcquireTokenAsync(Constants.ResourceUrl,
                    Constants.ClientIdForUserAuthn, redirectUri, new PlatformParameters(PromptBehavior.Always));
                TokenForUser = userAuthnResult.AccessToken;
                Console.WriteLine("\n Welcome " + userAuthnResult.UserInfo.GivenName + " " +
                                  userAuthnResult.UserInfo.FamilyName);
            }
            return TokenForUser;
        }

        /// <summary>
        /// Get Active Directory Client for User.
        /// </summary>
        /// <returns>ActiveDirectoryClient for User.</returns>
        public static ActiveDirectoryClient GetActiveDirectoryClientAsUser()
        {
            Uri servicePointUri = new Uri(Constants.ResourceUrl);
            Uri serviceRoot = new Uri(servicePointUri, Constants.TenantId);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot, GetTokenForUserAsync);
            return activeDirectoryClient;
        }
    }
}
