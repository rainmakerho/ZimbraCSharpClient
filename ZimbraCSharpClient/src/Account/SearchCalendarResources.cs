using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Zimbra.Client.Account;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Account
{
    public class SearchCalendarResourcesRequest : AccountServiceRequest
    {
        public string CalendarAttributes { get; set; }

        public Action<XmlElement> ProcToXmlDocument = null;

        public override String Name()
        {
            return AccountService.NS_PREFIX + ":" + AccountService.SEARCH_CALENDAR_RESOURCES_REQUEST;
        }

        public SearchCalendarResourcesRequest()
        {
            CalendarAttributes = AccountService.V_DEFAULT_CALENDAR_ATTRS;
             
        }

        public SearchCalendarResourcesRequest(string calendarAttributes)
        {
            CalendarAttributes = calendarAttributes;
            
        }
        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(AccountService.SEARCH_CALENDAR_RESOURCES_REQUEST, AccountService.NAMESPACE_URI);
            reqElem.SetAttribute(AccountService.A_ATTRS, CalendarAttributes);
            XmlElement searchFilterE = doc.CreateElement(AccountService.E_SEARCH_FILTER, AccountService.NAMESPACE_URI);
            XmlElement condsE = doc.CreateElement(AccountService.E_CONDS, AccountService.NAMESPACE_URI);
            if (ProcToXmlDocument == null)
            {
                //預設查詢 會議室
                XmlElement condE = doc.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
                condE.SetAttribute(AccountService.A_ATTR, AccountService.V_ATTR_CALRES_TYPE);
                condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE_EQ);
                condE.SetAttribute(AccountService.A_VALUE, AccountService.V_LOCATION);
                condsE.AppendChild(condE);
            }
            else
            {
                //查詢條件由外面傳入
                ProcToXmlDocument?.Invoke(condsE);
            }
            searchFilterE.AppendChild(condsE);
            reqElem.AppendChild(searchFilterE);
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class SearchCalendarResourcesResponse : Response
    {
        public List<CalendarResource> CalendarResourceList { get; set; }
        public SearchCalendarResourcesResponse()
        {}

        public SearchCalendarResourcesResponse(List<CalendarResource> calendarResourceList)
        {
            CalendarResourceList = calendarResourceList;
        }

        public override String Name
        {
            get { return AccountService.NS_PREFIX + ":" + AccountService.SEARCH_CALENDAR_RESOURCES_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var calresList = new List<CalendarResource>();
            foreach (XmlNode calResource in responseNode.ChildNodes)
            {
                var calres = new CalendarResource{id = XmlUtil.AttributeValue(calResource.Attributes,AccountService.A_ID)};
                foreach (XmlNode attrNode  in calResource.ChildNodes)
                {
                    calres.AttributesList.Add(XmlUtil.AttributeValue(attrNode.Attributes, AccountService.A_ATTRNAME), attrNode.InnerText);
                }
                calresList.Add(calres);
            }
            return new SearchCalendarResourcesResponse(calresList);
        }
    }

    public class CalendarResource
    {
        public string id { get; set; }

        public Dictionary<string, string> AttributesList;

        public CalendarResource()
        {
            AttributesList = new Dictionary<string, string>();
        }
    }
}
