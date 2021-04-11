using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TOC
{
    public partial class Login : System.Web.UI.Page
    {
        public static string AppName = "optionsforoptions";
        public static string ClientId = "1005286391235-fqacl5q72kj8462g71am80np6diudqp1.apps.googleusercontent.com";
        public static string ClientSecret = "4F33hC6adRgMc9n6YY8eZCCR";

        public static string[] Scopes =
        {
            //GoogleWebAuthorizationBroker.AuthorizeAsync()
            //Google.Apis.Auth.OAuth2.ServiceAccountCredential.
            //GmailService.Scope.GmailSend
        };

        public static UserCredential GetUserCredential(out string error)
        {
            UserCredential credential = null;
            error = string.Empty;

            try
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = ClientId,
                        ClientSecret = ClientSecret
                    },
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore("Google Oauth2")).Result;
            }
            catch (Exception ex)
            {
                credential = null;
                error = "Failed in user credential initialization: " + ex.ToString();
            }
            return credential;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAuthorize_Click(object sender, EventArgs e)
        {
            string credentialError = string.Empty;
            string refreshToken = string.Empty;

            UserCredential credential = GetUserCredential(out credentialError);
            if (credential != null && string.IsNullOrWhiteSpace(credentialError))
            {
                refreshToken = credential.Token.RefreshToken;
            }
        }
    }
}