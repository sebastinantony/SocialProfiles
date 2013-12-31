using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialProfiles
{
    public partial class Response : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            GoogleClient.Code = Request.QueryString["code"].ToString();
            lblCode.Text = GoogleClient.Code;
        }

        protected void btnToken_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("code", GoogleClient.Code);
            collection.Add("redirect_uri" , HttpUtility.UrlEncode(GoogleClient.RedirectURI) );
            collection.Add("client_id" , GoogleClient.ClientID );
            collection.Add("scope", "");
            collection.Add("client_secret", GoogleClient.ClientSecret);
            collection.Add("grant_type", "authorization_code");

            ltResponse.Text = "<h1>Response</h1><p>" + GetWebResponse("https://accounts.google.com/o/oauth2/token", collection) + "</p>";
            
        }

        static string GetWebResponse(string url, NameValueCollection parameters)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";

            var sb = new StringBuilder();
            foreach (var key in parameters.AllKeys)
                sb.Append(key + "=" + parameters[key] + "&");
            sb.Length = sb.Length - 1;

            byte[] requestBytes = Encoding.UTF8.GetBytes(sb.ToString());
            httpWebRequest.ContentLength = requestBytes.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            WebResponse response = httpWebRequest.GetResponse();

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, null);
            using (var responseStream = responseTask.Result.GetResponseStream())
            {
                var reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
        }
    }
}