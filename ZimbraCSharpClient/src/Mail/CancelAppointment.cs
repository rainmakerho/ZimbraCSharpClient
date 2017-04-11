using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Zimbra.Client.Mail;

namespace Zimbra.Client.src.Mail
{
    public class CancelAppointmentRequest : MailServiceRequest
    {
        private CancelAppointmentRequestParam _reqParam;

        public CancelAppointmentRequest()
        {}

        public CancelAppointmentRequest(CancelAppointmentRequestParam reqParam)
        {
            _reqParam = reqParam;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.CANCEL_APPT_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.CANCEL_APPT_REQUEST, MailService.NAMESPACE_URI);
            reqElem.SetAttribute(MailService.A_ID, _reqParam.Id);
            reqElem.SetAttribute(MailService.E_COMP, MailService.V_ZERO);

            XmlElement mElem = doc.CreateElement(MailService.E_MESSAGE, MailService.NAMESPACE_URI);

            //參與人員包含 會議室、Resources  
            if (_reqParam.Attendees != null)
            {
                foreach (var attendee in _reqParam.Attendees)
                {
                    var at = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
                    at.SetAttribute(MailService.A_EMAIL, attendee.Email);
                    at.SetAttribute(MailService.A_TYPE, MailService.V_TO);
                    mElem.AppendChild(at);
                }
            }
            
             

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


    public class CancelAppointmentResponse : Response
    {
        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.CANCEL_APPT_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
             
            var res = new CancelAppointmentResponse();
            res.ResponseNode = responseNode;
            return res;
        }
    }


    public class CancelAppointmentRequestParam
    {
        public string Id { get; set; }
        //人員、會議室、資源 等等 
        public List<Attendee> Attendees { get; set; }
 
        public string Subject { get; set; }

        public string Body { get; set; }
 
    }
}
