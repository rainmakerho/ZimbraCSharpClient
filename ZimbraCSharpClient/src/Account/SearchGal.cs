using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Zimbra.Client.Account;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Account
{
    public class SearchGalRequest :  AccountServiceRequest
    {
        public string SearchString { get; set; }

        public SearchGalRequest()
        {}

        public SearchGalRequest(string searchString)
        {
            SearchString = searchString;
        }

        public override String Name()
        {
            return AccountService.NS_PREFIX + ":" + AccountService.SEARCH_GAL_REQUEST;
        }

        

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(AccountService.SEARCH_GAL_REQUEST, AccountService.NAMESPACE_URI);
            reqElem.SetAttribute(AccountService.A_NAME, SearchString);
            reqElem.SetAttribute(AccountService.A_TYPE, AccountService.V_ATTR_ACCOUNT);
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class SearchGalResponse : Response
    {
        public List<CalendarResource> CalendarResourceList { get; set; }
        public SearchGalResponse()
        {}

        public SearchGalResponse(List<CalendarResource> calResources)
        {
            CalendarResourceList = calResources;
        }

        public override String Name
        {
            get { return AccountService.NS_PREFIX + ":" + AccountService.SEARCH_GAL_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var calresList = new List<CalendarResource>();

            foreach (XmlNode calResource in responseNode.ChildNodes)
            {
                var calres = new CalendarResource { id = XmlUtil.AttributeValue(calResource.Attributes, AccountService.A_ID) };
                foreach (XmlNode attrNode in calResource.ChildNodes)
                {
                    var attrName = XmlUtil.AttributeValue(attrNode.Attributes, AccountService.A_ATTRNAME);
                    if (!calres.AttributesList.ContainsKey(attrName))
                    {
                        calres.AttributesList.Add(attrName, attrNode.InnerText);
                    }
                    
                }
                calresList.Add(calres);
            }
            var res = new SearchGalResponse(calresList);
            res.ResponseNode = responseNode;
            return res;
        }
    }


     
}
