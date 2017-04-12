using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Zimbra.Client.Account;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class SendInviteReplyRequest : MailServiceRequest
    {
        private SendInviteReplyRequestParam _reqParam;

        public SendInviteReplyRequest(SendInviteReplyRequestParam reqParam)
        {
            _reqParam = reqParam;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.SEND_INVITE_REPLY_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.SEND_INVITE_REPLY_REQUEST, MailService.NAMESPACE_URI);
            reqElem.SetAttribute(MailService.A_ID, _reqParam.Id);
            reqElem.SetAttribute(MailService.A_COMP_NUM, MailService.V_ZERO);
            reqElem.SetAttribute(MailService.A_UPDATE_OGGANIZER, MailService.V_TRUE);
            reqElem.SetAttribute(MailService.A_VERB, _reqParam.Verb.ToString(MailService.EnumToStringFormat));

            XmlElement mElem = doc.CreateElement(MailService.E_MESSAGE, MailService.NAMESPACE_URI);

            //組織者 Organizer 
            var ogElem = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
            ogElem.SetAttribute(MailService.A_EMAIL, _reqParam.Organizer.Email);
            ogElem.SetAttribute(MailService.A_TYPE, MailService.V_TO);
            mElem.AppendChild(ogElem);

            //回覆者 Replier
            var rpElem = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
            rpElem.SetAttribute(MailService.A_EMAIL, _reqParam.Organizer.Email);
            rpElem.SetAttribute(MailService.A_TYPE, MailService.V_FROM);
            mElem.AppendChild(rpElem);

            var su = doc.CreateElement(MailService.E_SUBJECT, MailService.NAMESPACE_URI);
            su.InnerText = _reqParam.Subject;
            mElem.AppendChild(su);

            var mp = doc.CreateElement(MailService.E_MIME_PART, MailService.NAMESPACE_URI);
            mp.SetAttribute(MailService.A_CONTENT_TYPE, MailService.V_CT);

            var mpPlain = doc.CreateElement(MailService.E_MIME_PART, MailService.NAMESPACE_URI);
            mpPlain.SetAttribute(MailService.A_CONTENT_TYPE, MailService.V_TEXT_PLAIN);
            var contentPlain = doc.CreateElement(MailService.E_CONTENT, MailService.NAMESPACE_URI);
            contentPlain.InnerText = _reqParam.Body;
            mpPlain.AppendChild(contentPlain);
            mp.AppendChild(mpPlain);
            mElem.AppendChild(mp);

            reqElem.AppendChild(mElem);
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class SendInviteReplyResponse:Response
    {
        private string _invId;

        public SendInviteReplyResponse()
        {}

        public SendInviteReplyResponse(string invid)
        {
            _invId = invid;
        }

        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.SEND_INVITE_REPLY_REPONSE; }
        }

        public String InvId
        {
            get { return _invId; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var invId = XmlUtil.AttributeValue(responseNode.Attributes, MailService.A_INV_ID);
            var res = new SendInviteReplyResponse(invId);
            res.ResponseNode = responseNode;
            return res;
        }
    }

    public class SendInviteReplyRequestParam
    {
        public enum ReplyVerbs
        {
            ACCEPT,
            DECLINE,
            TENTATIVE
        }

        //會議 Id InvId
        public string Id { get; set; }

        //組織者
        public Attendee Organizer { get; set; }

        //回覆者
        public Attendee Replier { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public ReplyVerbs Verb { get; set; }
    }
}
