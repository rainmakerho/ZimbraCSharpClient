using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class CheckRecurConflictsRequest : MailServiceRequest
    {
        private DateTime _start; //localTime
        private DateTime _end;	//localTime
        private string _userNames; //多個以逗號隔開

        public CheckRecurConflictsRequest()
        {}

        public CheckRecurConflictsRequest(DateTime startDate, DateTime endDate, string userNames)
        {
            _start = startDate;
            _end = endDate;
            _userNames = userNames;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.CHECK_RECURCONFLICTS_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.CHECK_RECURCONFLICTS_REQUEST, MailService.NAMESPACE_URI);
             
            Int64 gmtStartMillis = DateUtil.DateTimeToGmtMillis(_start);
            Int64 gmtEndMillis = DateUtil.DateTimeToGmtMillis(_end);
            reqElem.SetAttribute(MailService.A_START, gmtStartMillis.ToString());
            reqElem.SetAttribute(MailService.A_END, gmtEndMillis.ToString());

            //add comp/s & comp/e
            var compElem = doc.CreateElement(MailService.E_COMP, MailService.NAMESPACE_URI);
            var startElem = doc.CreateElement(MailService.E_START, MailService.NAMESPACE_URI);
            startElem.SetAttribute(MailService.A_TIMEZONE, MailService.V_TIMEZONE_TAIPEI);
            startElem.SetAttribute(MailService.A_DATE, _start.ToString(MailService.DateTimeFormat));
            compElem.AppendChild(startElem);

            var endElem = doc.CreateElement(MailService.E_END, MailService.NAMESPACE_URI);
            endElem.SetAttribute(MailService.A_TIMEZONE, MailService.V_TIMEZONE_TAIPEI);
            endElem.SetAttribute(MailService.A_DATE, _end.ToString(MailService.DateTimeFormat));
            compElem.AppendChild(endElem);
            reqElem.AppendChild(compElem);

            foreach (var user in _userNames.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                var userElem = doc.CreateElement(MailService.E_USR, MailService.NAMESPACE_URI);
                userElem.SetAttribute(MailService.A_NAME, user);
                reqElem.AppendChild(userElem);
            }
            
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class CheckRecurConflictsResponse : Response
    {
        public List<string> BusyUsers;

        public CheckRecurConflictsResponse()
        {}

        public CheckRecurConflictsResponse(List<string> busyUsers)
        {
            BusyUsers = busyUsers;
        }

        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.CHECK_RECURCONFLICTS_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var busyUsers = new List<string>();
            //取得 Conflicts 的人員(會議室、車子...)
            var userNodes = responseNode.SelectNodes($"//{MailService.NS_PREFIX}:usr", XmlUtil.NamespaceManager);
            if (userNodes != null)
            {
                foreach (XmlNode userNode in userNodes)
                {
                    string email = XmlUtil.AttributeValue(userNode.Attributes, MailService.A_NAME);
                    busyUsers.Add(email);
                }
            }
            var res = new CheckRecurConflictsResponse(busyUsers);
            res.ResponseNode = responseNode;
            return res;
        }
    }
}
