﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class GetFreeBusyRequest : MailServiceRequest
    {
        private DateTime _start; //localTime
        private DateTime _end;	//localTime
        private string _searchNames;
        public GetFreeBusyRequest(DateTime localStart, DateTime localEnd, string searchNames)
        {
            this._start = localStart;
            this._end = localEnd;
            this._searchNames = searchNames;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.GET_FREE_BUSY_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.GET_FREE_BUSY_REQUEST, MailService.NAMESPACE_URI);
            reqElem.SetAttribute(MailService.A_NAME, _searchNames);
            Int64 gmtStartMillis = DateUtil.DateTimeToGmtMillis(_start);
            Int64 gmtEndMillis = DateUtil.DateTimeToGmtMillis(_end);
            reqElem.SetAttribute(MailService.A_START, gmtStartMillis.ToString());
            reqElem.SetAttribute(MailService.A_END, gmtEndMillis.ToString());
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class GetFreeBusyResponse : Response
    {
        public Workinghours Workinghours { get; set; }

        public GetFreeBusyResponse()
        { }
        public GetFreeBusyResponse(Workinghours workinghours)
        {
            this.Workinghours = workinghours;
        }
        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.GET_FREE_BUSY_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var workinghours = WorkinghoursUtil.ParseNodeInfo2Workinghours(responseNode);
             
            return new GetFreeBusyResponse(workinghours);
        }
    }
}
