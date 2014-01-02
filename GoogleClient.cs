using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialProfiles
{
    public class GoogleClient
    {
        public static string ClientID = "108169322756-pbgv2b3ng77akofik74airq8kffao5ou.apps.googleusercontent.com";
        public static string ClientSecret = "HIo_K3d1JwcK3TgvOavALGt8";
        public static string Scope = "&scope=" + HttpUtility.UrlEncode("https://www.google.com/m8/feeds/") + "&approval_prompt=force&access_type=offline";
        public static string RedirectURI = "https://socialprofiles.apphb.com/Response.aspx";
        public static string Code;
        public static string SignIn = "https://accounts.google.com/o/oauth2/auth?redirect_uri=";
        public static string GetContacts = "https://www.google.com/m8/feeds/contacts/default/full/";
    }
}