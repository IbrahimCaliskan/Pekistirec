using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GoogleAPI
{
    internal class GoogleRefreshToAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
    }

    public partial class Utils
    {
        private static IAuthorizationState GetCredentials(string refreshToken)
        {
            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("client_id={0}&", GoogleAPI.ClientCredentials.CLIENT_ID));
            postData.Append(String.Format("client_secret={0}&", GoogleAPI.ClientCredentials.CLIENT_SECRET));
            postData.Append(String.Format("refresh_token={0}&", refreshToken));
            postData.Append("grant_type=refresh_token");
            MyWebRequest request = new MyWebRequest("https://accounts.google.com/o/oauth2/token", "POST", postData.ToString());
            string data = request.GetResponse();

            var token = JsonConvert.DeserializeObject<GoogleRefreshToAccessToken>(data);

            AuthorizationState credentials = new AuthorizationState(ClientCredentials.SCOPES);
            credentials.Callback = new Uri(ClientCredentials.REDIRECT_URI);
            //credentials.AccessTokenExpirationUtc = token.expires_in;
            credentials.AccessToken = token.access_token;

            return credentials;
        }

        /// <summary>
        /// Retrieve credentials using the provided authorization code.
        ///
        /// This function exchanges the authorization code for an access token and
        /// queries the UserInfo API to retrieve the user's e-mail address. If a
        /// refresh token has been retrieved along with an access token, it is stored
        /// in the application database using the user's e-mail address as key. If no
        /// refresh token has been retrieved, the function checks in the application
        /// database for one and returns it if found or throws a NoRefreshTokenException
        /// with the authorization URL to redirect the user to.
        /// </summary>
        /// <param name="authorizationCode">Authorization code to use to retrieve an access token.</param>
        /// <param name="state">State to set to the authorization URL in case of error.</param>
        /// <returns>OAuth 2.0 credentials instance containing an access and refresh token.</returns>
        /// <exception cref="CodeExchangeException">
        /// An error occurred while exchanging the authorization code.
        /// </exception>
        /// <exception cref="NoRefreshTokenException">
        /// No refresh token could be retrieved from the available sources.
        /// </exception>
        private static IAuthorizationState GetCredentials(String authorizationCode, String state)
        {
            IAuthorizationState credentials = ExchangeCode(authorizationCode);
            return credentials;
        }

        /// <summary>
        /// Exchange an authorization code for OAuth 2.0 credentials.
        /// </summary>
        /// <param name="authorizationCode">Authorization code to exchange for OAuth 2.0 credentials.</param>
        /// <returns>OAuth 2.0 credentials.</returns>
        /// <exception cref="CodeExchangeException">An error occurred.</exception>
        private static IAuthorizationState ExchangeCode(String authorizationCode)
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, ClientCredentials.CLIENT_ID, ClientCredentials.CLIENT_SECRET);
            IAuthorizationState state = new AuthorizationState();
            state.Callback = new Uri(ClientCredentials.REDIRECT_URI);
            state = provider.ProcessUserAuthorization(authorizationCode, state);            
            return state;
        }

        /// <summary>
        /// Send a request to the UserInfo API to retrieve the user's information.
        /// </summary>
        /// <param name="credentials">OAuth 2.0 credentials to authorize the request.</param>
        /// <returns>User's information.</returns>
        /// <exception cref="NoUserIdException">An error occurred.</exception>
        private static Userinfo GetUserInfo(IAuthorizationState credentials)
        {
            return GetUserInfo(GetAuthenticatorFromState(credentials));
        }

        /// <summary>
        /// Send a request to the UserInfo API to retrieve the user's information.
        /// </summary>
        /// <param name="credentials">OAuth 2.0 credentials to authorize the request.</param>
        /// <returns>User's information.</returns>
        /// <exception cref="NoUserIdException">An error occurred.</exception>
        private static Userinfo GetUserInfo(IAuthenticator credentials)
        {
            Oauth2Service userInfoService = new Oauth2Service(credentials);
            Userinfo userInfo = null;
            userInfo = userInfoService.Userinfo.Get().Fetch();
            return userInfo;
        }

        private static IAuthenticator GetAuthenticator(IAuthorizationState credentials)
        {
            IAuthenticator auth = GetAuthenticatorFromState(credentials);
            return auth;
        }

        /// <summary>
        /// Retrieve an IAuthenticator instance using the provided state.
        /// </summary>
        /// <param name="credentials">OAuth 2.0 credentials to use.</param>
        /// <returns>Authenticator using the provided OAuth 2.0 credentials</returns>
        private static IAuthenticator GetAuthenticatorFromState(IAuthorizationState credentials)
        {
            var provider = new StoredStateClient(GoogleAuthenticationServer.Description, ClientCredentials.CLIENT_ID, ClientCredentials.CLIENT_SECRET, credentials);
            var auth = new OAuth2Authenticator<StoredStateClient>(provider, StoredStateClient.GetState);
            auth.LoadAccessToken();
            return auth;
        }

        /// <summary>
        /// Retrieve the authorization URL.
        /// </summary>
        /// <param name="emailAddress">User's e-mail address.</param>
        /// <param name="state">State for the authorization URL.</param>
        /// <returns>Authorization URL to redirect the user to.</returns>
        private static String GetAuthorizationUrl(bool RefreshTokenLazim = true)
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = ClientCredentials.CLIENT_ID;

            IAuthorizationState authorizationState = new AuthorizationState(ClientCredentials.SCOPES);
            authorizationState.Callback = new Uri(ClientCredentials.REDIRECT_URI);

            UriBuilder builder = new UriBuilder(provider.RequestUserAuthorization(authorizationState));
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(builder.Query);
            
            if (RefreshTokenLazim)
            {
                queryParameters.Set("access_type", "offline");
                queryParameters.Set("approval_prompt", "force");
            }                
            else
                queryParameters.Set("approval_prompt", "auto");

            builder.Query = queryParameters.ToString();
            return builder.Uri.ToString();
        }

        /// <summary>
        /// Retrieve a list of File resources.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <returns>List of File resources.</returns>
        public static List<File> retrieveAllFiles(DriveService service)
        {
            List<File> result = new List<File>();
            FilesResource.ListRequest request = service.Files.List();

            do
            {
                try
                {
                    FileList files = request.Fetch();

                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return result;
        }

    }

}