using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Drive.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2.Data;

namespace GoogleAPI
{
    public partial class Utils
    {
        private static string _AuthorizationUrlRefreshToken = null;
        private static string _AuthorizationUrl = null;

        public static string AuthorizationUrl(bool RefreshTokenLazim = true)
        {
            if (RefreshTokenLazim)
            {
                if (_AuthorizationUrlRefreshToken == null)
                {
                    _AuthorizationUrlRefreshToken = GetAuthorizationUrl(true);
                }
                return _AuthorizationUrlRefreshToken;
            }
            else
            {
                if (_AuthorizationUrl == null)
                {
                    _AuthorizationUrl = GetAuthorizationUrl(false);                    
                }
                return _AuthorizationUrl;
            }            
        }

        private GoogleAPICarrier _Login(IAuthorizationState credentials)
        {
            #region Google üzerinde oturum açılıyor

            var auth = GetAuthenticator(credentials);
            var userInfo = GetUserInfo(credentials);
            var driveService = Drive.Utils.BuildService(auth);

            #endregion

            string parentId = null;

            #region Dosyaların atılacağı root klasörün ID bilgisi alınıyor.

            #region Mevcut klasörlerin içinde Pekiştireç klasörü aranıyor

            var liste = driveService.Files.List().Fetch();
            foreach (var item in liste.Items)
            {
                if ((item.MimeType == "application/vnd.google-apps.folder") &&
                    (item.Title == "Pekiştireç") &&
                    (item.ExplicitlyTrashed == null))
                {
                    parentId = item.Id;
                    break;
                }
            }

            #endregion

            #region Mevcut klasörlerin içinde Pekiştireç klasörü yoksa oluşturuluyor

            if (parentId == null)
            {
                parentId = (GoogleAPI.Drive.Utils.createFolder(driveService, "Pekiştireç", "Pekiştireç")).Id;
            }

            #endregion

            #endregion            

            return new GoogleAPICarrier()
            {
                auth = auth,
                credentials = credentials,
                driveService = driveService,
                parentId = parentId,
                userInfo = userInfo
            };
        }

        public GoogleAPICarrier Login(string RefreshToken)
        {
            var credentials = GetCredentials(RefreshToken);
            return _Login(credentials);
        }

        public GoogleAPICarrier Login(string code, string state = "")
        {
            var credentials = GetCredentials(code, state);
            return _Login(credentials);
        }

        public enum UploadPermission
        {
            OnlyMe,
            EveryoneRead,
            EveryoneWrite,
        }

        public  string Upload(GoogleAPICarrier carrier, string localFilePath, string title, string mime, UploadPermission uploadPermission = UploadPermission.EveryoneRead)
        {
            File file = Drive.Utils.insertFile(carrier.driveService, title, title, carrier.parentId, mime, localFilePath);
            Permission permission = new Permission();
            permission.Type = "anyone";
            permission.Value = "";

            switch (uploadPermission)
            {
                case UploadPermission.EveryoneRead: permission.Role = "reader";
                    break;
                case UploadPermission.EveryoneWrite: permission.Role = "writer";
                    break;                
            }
            if (uploadPermission != UploadPermission.OnlyMe)
                carrier.driveService.Permissions.Insert(permission, file.Id).Fetch();

            return file.AlternateLink;
        }

        public  string DokumanLinkGetir(string title, GoogleAPICarrier carrier)
        {
            List<File> TumDosyalar = GoogleAPI.Utils.retrieveAllFiles(carrier.driveService);
            IEnumerable<File> PekistirecDosyalari = from f in TumDosyalar
                                                    where f.Parents.Any(t => t.Id == carrier.parentId)
                                                    select f;

            File file = (from f in PekistirecDosyalari
                         where f.Title == title
                         select f).OrderByDescending(f => f.Title).FirstOrDefault();

            if (file != null)
            {
                return file.AlternateLink;
            }
            else
                return null;
        }

        public List<Drive.SimpleDriveFile> DriveFileListesiGetir(GoogleAPICarrier carrier)
        {
            List<Google.Apis.Drive.v2.Data.File> TumDosyalar = GoogleAPI.Utils.retrieveAllFiles(carrier.driveService);
            List<Google.Apis.Drive.v2.Data.File> PekistirecDosyalari = (from f in TumDosyalar
                                              where f.Parents.Any(t => t.Id == carrier.parentId)
                                              select f).ToList();
            List<Drive.SimpleDriveFile> dokumanlar = new List<Drive.SimpleDriveFile>();

            PekistirecDosyalari.ToList().ForEach(df => dokumanlar.Add(new Drive.SimpleDriveFile(df)));

            return dokumanlar;
        }
    }
}
