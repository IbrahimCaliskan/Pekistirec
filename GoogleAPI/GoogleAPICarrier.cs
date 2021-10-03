using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Drive.v2;
using Google.Apis.Oauth2.v2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAPI
{
    public class GoogleAPICarrier
    {
        public IAuthorizationState credentials { get; set; }
        public IAuthenticator auth { get; set; }
        public Userinfo userInfo { get; set; }
        public DriveService driveService { get; set; }
        public string parentId { get; set; }

        public string Id
        {
            get
            {
                return userInfo.Id;
            }
        }
        public string Email
        {
            get
            {
                return userInfo.Email;
            }
        }
        public string Name
        {
            get
            {
                return userInfo.Name;
            }
        }

        public string RefreshToken
        {
            get
            {
                return credentials.RefreshToken;
            }
        }
    }
}
