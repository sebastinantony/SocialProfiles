using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace SocialProfiles
{
    public partial class XmlTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XDocument doc = XDocument.Load(@"C:\Users\aantony\Documents\GitHub\SocialProfiles\ListOfContacts.xml");
            var query = from c in doc.Descendants("entry")
                        select c.Element("title").Value;
            foreach (string name in query)
            {
                Response.Write(name);
            }
        }
    }
}