﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialProfiles
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSign_Click(object sender, EventArgs e)
        {
            Response.Redirect(GoogleClient.SignIn + HttpUtility.UrlEncode(GoogleClient.RedirectURI )+ "&response_type=code&client_id=" + GoogleClient.ClientID + GoogleClient.Scope);
        }
    }
}