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
            var doc = XDocument.Load(@"C:\Users\aantony\Documents\GitHub\SocialProfiles\ListOfContacts.xml");

            XNamespace ns = "http://www.w3.org/2005/Atom";
            IEnumerable<XElement> node = doc.Descendants(ns + "entry");
            
            var query = from r in doc.Descendants(ns + "entry")
                        select new
                        {
                            Title = r.Element(ns + "title").Value

                        };
            foreach (var name in query)
            {
                Response.Write(name.Title);
            }
        }
    }
}