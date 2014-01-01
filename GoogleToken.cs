using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SocialProfiles
{
    public class GoogleToken
    {
        public string Access_token;
        public string Token_type;
        public int Expires_in;
        public string Refresh_token;

        //Empty Constructor
        public GoogleToken() { }
        public GoogleToken (string access_token , string token_type , string expries_in , string refresh_token) 
        {
            Access_token = access_token;
            Token_type = token_type;
            //Expires_in = int.Parse(expries_in);
            Refresh_token = refresh_token;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static GoogleToken  FromJson(string json)
        {
            var googletoken = JsonConvert.DeserializeObject<GoogleToken>(json);
            
            return googletoken;            
        }
    }
}