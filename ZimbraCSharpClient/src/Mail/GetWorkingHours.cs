using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class GetWorkingHoursRequest : MailServiceRequest
    {
        private DateTime start; //localTime
        private DateTime end;	//localTime
        private string searchNames;
        public GetWorkingHoursRequest(DateTime localStart, DateTime localEnd, string searchNames)
        {
            this.start = localStart;
            this.end = localEnd;
            this.searchNames = searchNames;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.GET_WORKINGHOURS_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.GET_WORKINGHOURS_REQUEST, MailService.NAMESPACE_URI);
            reqElem.SetAttribute(MailService.A_NAME, searchNames);
            Int64 gmtStartMillis = DateUtil.DateTimeToGmtMillis(start);
            Int64 gmtEndMillis = DateUtil.DateTimeToGmtMillis(end);
            reqElem.SetAttribute(MailService.A_START, gmtStartMillis.ToString());
            reqElem.SetAttribute(MailService.A_END, gmtEndMillis.ToString());
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class GetWorkingHoursResponse : Response
    {
        public Workinghours Workinghours { get; set; }

        public GetWorkingHoursResponse()
        {}
        public GetWorkingHoursResponse(Workinghours workinghours)
        {
            this.Workinghours = workinghours;
        }
        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.GET_WORKINGHOURS_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var workinghours = WorkinghoursUtil.ParseNodeInfo2Workinghours(responseNode);
            return new GetWorkingHoursResponse(workinghours);
        }


        
    }

    public class WorkinghoursUtil
    {
        public static Workinghours ParseNodeInfo2Workinghours(XmlNode responseNode)
        {
            var workinghours = new Workinghours();
            var usrNodes = responseNode.SelectNodes(MailService.NS_PREFIX + ":" + MailService.E_USR, XmlUtil.NamespaceManager);
            foreach (XmlNode usrNode in usrNodes)
            {
                var usr = new Usr(XmlUtil.AttributeValue(usrNode.Attributes, MailService.A_ID));
                var usrFNodes = usrNode.SelectNodes(MailService.NS_PREFIX + ":" + MailService.E_F, XmlUtil.NamespaceManager);
                for (int i = 0; i < usrFNodes.Count; i++)
                {
                    XmlNode iNode = usrFNodes.Item(i);
                    String s = XmlUtil.AttributeValue(iNode.Attributes, MailService.A_START);
                    Int64 seconds = Int64.Parse(s);
                    DateTime start = DateUtil.GmtSecondsToLocalTime(seconds);
                    String e = XmlUtil.AttributeValue(iNode.Attributes, MailService.A_END);
                    seconds = Int64.Parse(e);
                    DateTime end = DateUtil.GmtSecondsToLocalTime(seconds);
                    usr.Fs.Add(new Duration { s = start, e = end });
                }

                var usrBNodes = usrNode.SelectNodes(MailService.NS_PREFIX + ":" + MailService.E_B, XmlUtil.NamespaceManager);
                for (int i = 0; i < usrBNodes.Count; i++)
                {
                    XmlNode iNode = usrBNodes.Item(i);
                    String s = XmlUtil.AttributeValue(iNode.Attributes, MailService.A_START);
                    Int64 seconds = Int64.Parse(s);
                    DateTime start = DateUtil.GmtSecondsToLocalTime(seconds);
                    String e = XmlUtil.AttributeValue(iNode.Attributes, MailService.A_END);
                    seconds = Int64.Parse(e);
                    DateTime end = DateUtil.GmtSecondsToLocalTime(seconds);
                    usr.Bs.Add(new Duration { s = start, e = end });
                }

                var usrUNodes = usrNode.SelectNodes(MailService.NS_PREFIX + ":" + MailService.E_U, XmlUtil.NamespaceManager);
                for (int i = 0; i < usrUNodes.Count; i++)
                {
                    XmlNode iNode = usrUNodes.Item(i);
                    String s = XmlUtil.AttributeValue(iNode.Attributes, MailService.A_START);
                    Int64 seconds = Int64.Parse(s);
                    DateTime start = DateUtil.GmtSecondsToLocalTime(seconds);
                    String e = XmlUtil.AttributeValue(iNode.Attributes, MailService.A_END);
                    seconds = Int64.Parse(e);
                    DateTime end = DateUtil.GmtSecondsToLocalTime(seconds);
                    usr.Us.Add(new Duration { s = start, e = end });
                }
                workinghours.Users.Add(usr);
            }
            return workinghours;
        }
    }


    public class Workinghours
    {
        public Workinghours()
        {
            this.Users = new List<Usr>();
        }
        public IList<Usr> Users { get; set; }
    }

    public class Usr
    {

        public string id { get; set; }
        public IList<Duration> Us { get; set; }
        public IList<Duration> Fs { get; set; }

        public IList<Duration> Bs { get; set; }

        public Usr(string id)
        {
            this.id = id;
            this.Us = new List<Duration>();
            this.Fs = new List<Duration>();
            this.Bs = new List<Duration>();
        }
    }

    public class Duration
    {
        public DateTime s { get; set; }
        public DateTime e { get; set; }
    }
 
}
