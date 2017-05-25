using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class AutoCompleteRequest : MailServiceRequest
    {
        private string _Name;

        public AutoCompleteRequest()
        {}

        public AutoCompleteRequest(string searchName)
        {
            _Name = searchName;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.AUTO_COMPLETE_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.AUTO_COMPLETE_REQUEST, MailService.NAMESPACE_URI);
            reqElem.SetAttribute(MailService.A_NAME, _Name);
            reqElem.SetAttribute(MailService.A_NEED_EXP, "1");
            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class AutoCompleteResponse : Response
    {
        public List<Attendee> MatchList { get; set; }  

        public AutoCompleteResponse()
        {}

        public AutoCompleteResponse(List<Attendee> matchList)
        {
            MatchList = matchList;
        }

        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.AUTO_COMPLETE_REPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
             
            //match
            //email="rainmaker ho (亂馬客)" <rainmaker_ho@gss.com.tw>
            //取得所有match的內容
            var matchsList = new List<Attendee>();
            //取得 Conflicts 的人員(會議室、車子...)
            var matchNodes = responseNode.SelectNodes($"//{MailService.NS_PREFIX}:match", XmlUtil.NamespaceManager);
            if (matchNodes != null)
            {
                foreach (XmlNode matchNode in matchNodes)
                {
                    var email = XmlUtil.AttributeValue(matchNode.Attributes, "email");
                    var username = Regex.Match(email, "\"(.*)\"").Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var usermail = Regex.Match(email, @"<(.*)>").Groups[1].Value;
                        var user = new Attendee();
                        user.DisplayName = username;
                        user.Email = usermail;
                        matchsList.Add(user);
                    }
                }
            }
            var res = new AutoCompleteResponse(matchsList);
            res.ResponseNode = responseNode;
            return res;
        }
    }
 
}
