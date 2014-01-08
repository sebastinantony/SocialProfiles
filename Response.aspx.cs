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
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace SocialProfiles
{
    public partial class Response : System.Web.UI.Page
    {
        public string accessToken;
        private static IAsyncResult asyncResult;
        public GoogleToken googleToken
        {
            get;
            set;
        }

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
            accessToken = GetWebResponse("https://accounts.google.com/o/oauth2/token", collection);
            googleToken =  GoogleToken.FromJson(accessToken);
            Session["googleToken"] = googleToken.Access_token;
            ltResponse.Text = "<h1>Response</h1><p>" + accessToken + "</p>";
            
        }

        static string GetWebResponse(string url, NameValueCollection parameters)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
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

        static string GetContactDetails (string url , WebHeaderCollection parameters)
        {
            HttpWebRequest request = null;
            request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers = parameters;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream =  response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string result = reader.ReadToEnd();
            return result;
            
        }

        protected void btnListContacts_Click(object sender, EventArgs e)
        {
            WebHeaderCollection collections = new WebHeaderCollection();
            string key = "Bearer" + " " + Session["googleToken"].ToString();
            collections.Add("Authorization", key);

            string strXml = GetContactDetails(GoogleClient.GetContacts, collections);
            ReadXml(strXml);
        }

        private void ReadXml(string strXml)
        {
            StringBuilder str = new StringBuilder();
            XDocument doc = XDocument.Load(new StringReader(strXml));
            XNamespace ns = "http://www.w3.org/2005/Atom";
            var contactList = from c in doc.Descendants(ns + "entry")
                              select new
                              {
                                  Title = c.Element(ns + "title").Value
                              };

            foreach (var contact in contactList)
            {
                str.AppendLine(contact.Title);
            }
            ltContactList.Text =  str.ToString();
        } 
    }
}